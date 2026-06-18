/**
 * site.js — BmWms global auth & UI helpers
 * Được load ở cuối _Layout.cshtml, chạy trên mọi trang.
 */

// ── Auth object: lưu/đọc/xóa JWT token trong sessionStorage ──────────────────
const Auth = (() => {
    const KEY_TOKEN = 'bm_token';
    const KEY_REFRESH = 'bm_refresh';
    const KEY_ROLES = 'bm_roles';
    const KEY_NAME = 'bm_name';

    return {
        save(data) {
            sessionStorage.setItem(KEY_TOKEN, data.token);
            sessionStorage.setItem(KEY_REFRESH, data.refreshToken);
            sessionStorage.setItem(KEY_ROLES, JSON.stringify(data.roles ?? []));
            try {
                const payload = JSON.parse(atob(data.token.split('.')[1]));
                const fullName = payload['FullName'] || payload['unique_name'] || '';
                sessionStorage.setItem(KEY_NAME, fullName);
            } catch { sessionStorage.setItem(KEY_NAME, ''); }
        },
        getToken() { return sessionStorage.getItem(KEY_TOKEN); },
        getRefresh() { return sessionStorage.getItem(KEY_REFRESH); },
        getRoles() { try { return JSON.parse(sessionStorage.getItem(KEY_ROLES) || '[]'); } catch { return []; } },
        getName() { return sessionStorage.getItem(KEY_NAME) || ''; },
        hasRole(r) { return this.getRoles().includes(r); },
        isLoggedIn() { return !!this.getToken(); },
        clear() {
            sessionStorage.removeItem(KEY_TOKEN);
            sessionStorage.removeItem(KEY_REFRESH);
            sessionStorage.removeItem(KEY_ROLES);
            sessionStorage.removeItem(KEY_NAME);
        }
    };
})();

// ── Navigation helpers ────────────────────────────────────────────────────────
function redirectToLogin() { window.location.href = '/Auth/Login'; }
function redirectToDashboard() { window.location.href = '/Dashboard'; }

// ── requireAuth: gọi ở đầu mọi trang cần đăng nhập ──────────────────────────
function requireAuth() {
    if (!Auth.isLoggedIn()) {
        redirectToLogin();
        return false;
    }
    _renderUserInfo();
    _applyRoleVisibility();
    return true;
}

// ── logout ────────────────────────────────────────────────────────────────────
async function logout() {
    try {
        const refreshToken = Auth.getRefresh();
        if (refreshToken) {
            await fetch('/api/auth/logout', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + Auth.getToken()
                },
                body: JSON.stringify({ refreshToken })
            });
        }
    } catch { /* bỏ qua lỗi mạng khi logout */ }
    finally {
        Auth.clear();
        redirectToLogin();
    }
}

// ── apiFetch: fetch wrapper tự gắn Bearer token ──────────────────────────────
async function apiFetch(url, options = {}) {
    const token = Auth.getToken();
    options.headers = {
        'Content-Type': 'application/json',
        ...(token ? { 'Authorization': 'Bearer ' + token } : {}),
        ...(options.headers || {})
    };
    const res = await fetch(url, options);
    if (res.status === 401) {
        Auth.clear();
        redirectToLogin();
    }
    return res;
}

// ── Private: điền tên user vào sidebar ───────────────────────────────────────
function _renderUserInfo() {
    const nameEl = document.getElementById('sidebar-user-name');
    const avatarEl = document.getElementById('sidebar-user-avatar');
    const roleEl = document.getElementById('sidebar-user-role');
    if (!nameEl) return;

    const name = Auth.getName();
    const roles = Auth.getRoles();

    if (nameEl) nameEl.textContent = name || 'Người dùng';
    if (avatarEl) avatarEl.textContent = name ? name.charAt(0).toUpperCase() : '?';
    if (roleEl) roleEl.textContent = roles.join(', ');
}

// ── Private: ẩn các phần tử cần role cụ thể ──────────────────────────────────
function _applyRoleVisibility() {
    document.querySelectorAll('[data-require-role]').forEach(el => {
        const required = el.dataset.requireRole;
        if (!Auth.hasRole(required)) el.style.display = 'none';
    });
}

// ── Auto-requireAuth cho mọi trang KHÔNG phải NoShell ────────────────────────
(function () {
    const noAuthPaths = ['/auth/login', '/auth/forgotpassword', '/auth/changepassword'];
    const currentPath = window.location.pathname.toLowerCase();
    const isPublicPage = noAuthPaths.some(p => currentPath.startsWith(p));
    if (!isPublicPage) {
        requireAuth();
    }
})();


// ════════════════════════════════════════════════════════════════════════════
// PROFILE & USER DROPDOWN
// ════════════════════════════════════════════════════════════════════════════

let _profileCache = null;

// Load thông tin user lên topbar
async function loadTopbarUser() {
    try {
        const res = await apiFetch('/api/users/me');
        if (!res.ok) return;
        const p = await res.json();
        _profileCache = p;
        const nameEl = document.getElementById('topbarUserName');
        const roleEl = document.getElementById('topbarUserRole');
        const avatarEl = document.getElementById('topbarAvatar');
        if (nameEl) nameEl.textContent = p.fullName || p.username;
        if (roleEl) roleEl.textContent = (p.role && p.role !== '—') ? p.role : p.department;
        if (avatarEl) avatarEl.src = 'https://ui-avatars.com/api/?name='
            + encodeURIComponent(p.fullName) + '&background=2563eb&color=fff';
    } catch (e) { /* silent */ }
}

// Dropdown toggle
function toggleUserDropdown() {
    const menu = document.getElementById('userDropdownMenu');
    if (menu) menu.classList.toggle('show');
}

// Đóng dropdown khi click ra ngoài
document.addEventListener('click', function (e) {
    const btn = document.getElementById('userProfileBtn');
    const menu = document.getElementById('userDropdownMenu');
    if (!btn || !menu) return;
    const wrapper = btn.closest('.user-dropdown-wrapper');
    if (wrapper && !wrapper.contains(e.target)) menu.classList.remove('show');
});

// Mở modal View Profile
async function openViewProfile() {
    const menu = document.getElementById('userDropdownMenu');
    if (menu) menu.classList.remove('show');

    let p = _profileCache;
    if (!p) {
        const res = await apiFetch('/api/users/me');
        if (!res.ok) { alert('Không thể tải thông tin.'); return; }
        p = await res.json();
        _profileCache = p;
    }

    const avatarSrc = 'https://ui-avatars.com/api/?name='
        + encodeURIComponent(p.fullName) + '&background=2563eb&color=fff&size=80';

    const setEl = (id, val, isImg) => {
        const el = document.getElementById(id);
        if (!el) return;
        if (isImg) el.src = val; else el.textContent = val;
    };

    setEl('vpAvatar', avatarSrc, true);
    setEl('vpFullName', p.fullName);
    setEl('vpRole', (p.role && p.role !== '—') ? p.role : p.department);
    setEl('vpEmployeeId', p.employeeId);
    setEl('vpUsername', p.username);
    setEl('vpEmail', p.email || '—');
    setEl('vpPhone', p.phoneNumber || '—');
    setEl('vpDepartment', p.department);
    setEl('vpJoinDate', p.joinDate);

    const statusEl = document.getElementById('vpStatus');
    if (statusEl) {
        statusEl.innerHTML = p.status === 'Active'
            ? '<span class="badge-status-active">\u25CF Active</span>'
            : '<span class="badge-status-inactive">\u25CF Inactive</span>';
    }

    new bootstrap.Modal(document.getElementById('viewProfileModal')).show();
}

// Mở modal Update Profile
function openUpdateProfile() {
    const vpModal = bootstrap.Modal.getInstance(document.getElementById('viewProfileModal'));
    if (vpModal) vpModal.hide();

    const p = _profileCache;
    if (!p) return;

    const avatarSrc = 'https://ui-avatars.com/api/?name='
        + encodeURIComponent(p.fullName) + '&background=2563eb&color=fff&size=80';

    const setVal = (id, val, isSrc) => {
        const el = document.getElementById(id);
        if (!el) return;
        if (isSrc) el.src = val; else el.value = val;
    };

    setVal('upAvatar', avatarSrc, true);
    setVal('upEmployeeId', p.employeeId);
    setVal('upFullName', p.fullName);
    setVal('upEmail', p.email || '');
    setVal('upPhone', p.phoneNumber || '');
    setVal('upDepartment', p.department);
    setVal('upRole', (p.role && p.role !== '—') ? p.role : p.department);
    setVal('upStatus', p.status);

    ['upFullNameErr', 'upEmailErr', 'upPhoneErr', 'upGlobalErr', 'upGlobalOk'].forEach(id => {
        const el = document.getElementById(id);
        if (el) { el.style.display = 'none'; el.textContent = ''; }
    });

    setTimeout(() => new bootstrap.Modal(document.getElementById('updateProfileModal')).show(), 200);
}

// Lưu profile
async function saveProfile() {
    const fullName = (document.getElementById('upFullName').value || '').trim();
    const email = (document.getElementById('upEmail').value || '').trim();
    const phone = (document.getElementById('upPhone').value || '').trim();
    let valid = true;

    ['upFullNameErr', 'upEmailErr', 'upPhoneErr', 'upGlobalErr', 'upGlobalOk'].forEach(id => {
        const el = document.getElementById(id);
        if (el) { el.style.display = 'none'; el.textContent = ''; }
    });

    if (!fullName) { _showMsg('upFullNameErr', 'Họ tên không được để trống.'); valid = false; }
    if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        _showMsg('upEmailErr', 'Email không hợp lệ.'); valid = false;
    }
    if (!valid) return;

    const btn = document.getElementById('upSaveBtn');
    btn.disabled = true; btn.textContent = 'Đang lưu...';

    try {
        const res = await apiFetch('/api/users/me', {
            method: 'PUT',
            body: JSON.stringify({ fullName, email: email || null, phoneNumber: phone || null })
        });
        const data = await res.json();
        if (!res.ok) {
            _showMsg('upGlobalErr', data.message || data.title || 'Có lỗi xảy ra.');
        } else {
            _profileCache = null;
            await loadTopbarUser();
            _showMsg('upGlobalOk', 'Cập nhật thông tin thành công!');
            setTimeout(() => {
                const m = bootstrap.Modal.getInstance(document.getElementById('updateProfileModal'));
                if (m) m.hide();
            }, 1200);
        }
    } catch (e) {
        _showMsg('upGlobalErr', 'Lỗi kết nối. Vui lòng thử lại.');
    } finally {
        btn.disabled = false;
        btn.innerHTML = '<i class="bi bi-check-lg me-1"></i>Save Changes';
    }
}

// Mở modal Change Password
function openChangePassword() {
    const menu = document.getElementById('userDropdownMenu');
    if (menu) menu.classList.remove('show');

    ['cpCurrent', 'cpNew', 'cpConfirm'].forEach(id => {
        const f = document.getElementById(id); if (f) f.value = '';
    });
    ['cpCurrentErr', 'cpNewErr', 'cpConfirmErr', 'cpGlobalErr', 'cpGlobalOk'].forEach(id => {
        const el = document.getElementById(id);
        if (el) { el.style.display = 'none'; el.textContent = ''; }
    });

    new bootstrap.Modal(document.getElementById('changePasswordModal')).show();
}

// Thực hiện đổi mật khẩu
async function doChangePassword() {
    const current = document.getElementById('cpCurrent').value;
    const nw = document.getElementById('cpNew').value;
    const confirm = document.getElementById('cpConfirm').value;
    let valid = true;

    ['cpCurrentErr', 'cpNewErr', 'cpConfirmErr', 'cpGlobalErr', 'cpGlobalOk'].forEach(id => {
        const el = document.getElementById(id);
        if (el) { el.style.display = 'none'; el.textContent = ''; }
    });

    if (!current) { _showMsg('cpCurrentErr', 'Vui lòng nhập mật khẩu hiện tại.'); valid = false; }
    const pwRx = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    if (!nw) { _showMsg('cpNewErr', 'Vui lòng nhập mật khẩu mới.'); valid = false; }
    else if (!pwRx.test(nw)) { _showMsg('cpNewErr', 'Mật khẩu cần ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.'); valid = false; }
    if (nw && confirm !== nw) { _showMsg('cpConfirmErr', 'Xác nhận mật khẩu không khớp.'); valid = false; }
    if (!valid) return;

    const btn = document.getElementById('cpSaveBtn');
    btn.disabled = true; btn.textContent = 'Đang cập nhật...';

    try {
        const res = await apiFetch('/api/auth/change-password', {
            method: 'POST',
            body: JSON.stringify({ currentPassword: current, newPassword: nw })
        });
        const data = await res.json();
        if (!res.ok) {
            _showMsg('cpGlobalErr', data.message || 'Có lỗi xảy ra.');
        } else {
            _showMsg('cpGlobalOk', 'Đổi mật khẩu thành công! Đang chuyển hướng đăng nhập...');
            setTimeout(() => { Auth.clear(); redirectToLogin(); }, 1800);
        }
    } catch (e) {
        _showMsg('cpGlobalErr', 'Lỗi kết nối. Vui lòng thử lại.');
    } finally {
        btn.disabled = false;
        btn.innerHTML = '<i class="bi bi-shield-check me-1"></i>Update Password';
    }
}

// Logout qua dropdown
async function doLogout() {
    await logout();
}

// Toggle hiện/ẩn mật khẩu
function togglePw(inputId, iconEl) {
    const inp = document.getElementById(inputId);
    if (!inp) return;
    if (inp.type === 'password') {
        inp.type = 'text';
        iconEl.querySelector('i').className = 'bi bi-eye-slash';
    } else {
        inp.type = 'password';
        iconEl.querySelector('i').className = 'bi bi-eye';
    }
}

// Helper hiển thị thông báo
function _showMsg(id, msg) {
    const el = document.getElementById(id);
    if (el) { el.textContent = msg; el.style.display = 'block'; }
}

// Tự load topbar user sau khi DOM ready
document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('topbarUserName')) {
        loadTopbarUser();
    }
});