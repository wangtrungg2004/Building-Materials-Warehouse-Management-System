///**
// * site.js — BmWms global auth & UI helpers
// * Được load ở cuối _Layout.cshtml, chạy trên mọi trang.
// */

//// ── Auth object: lưu/đọc/xóa JWT token trong sessionStorage ──────────────────
//const Auth = (() => {
//    const KEY_TOKEN   = 'bm_token';
//    const KEY_REFRESH = 'bm_refresh';
//    const KEY_ROLES   = 'bm_roles';
//    const KEY_NAME    = 'bm_name';

//    return {
//        save(data) {
//            sessionStorage.setItem(KEY_TOKEN,   data.token);
//            sessionStorage.setItem(KEY_REFRESH, data.refreshToken);
//            sessionStorage.setItem(KEY_ROLES,   JSON.stringify(data.roles ?? []));
//            // Decode FullName từ JWT payload
//            try {
//                const payload = JSON.parse(atob(data.token.split('.')[1]));
//                const fullName = payload['FullName'] || payload['unique_name'] || '';
//                sessionStorage.setItem(KEY_NAME, fullName);
//            } catch { sessionStorage.setItem(KEY_NAME, ''); }
//        },
//        getToken()   { return sessionStorage.getItem(KEY_TOKEN); },
//        getRefresh() { return sessionStorage.getItem(KEY_REFRESH); },
//        getRoles()   { try { return JSON.parse(sessionStorage.getItem(KEY_ROLES) || '[]'); } catch { return []; } },
//        getName()    { return sessionStorage.getItem(KEY_NAME) || ''; },
//        hasRole(r)   { return this.getRoles().includes(r); },
//        isLoggedIn() { return !!this.getToken(); },
//        clear() {
//            sessionStorage.removeItem(KEY_TOKEN);
//            sessionStorage.removeItem(KEY_REFRESH);
//            sessionStorage.removeItem(KEY_ROLES);
//            sessionStorage.removeItem(KEY_NAME);
//        }
//    };
//})();

//// ── Navigation helpers ────────────────────────────────────────────────────────
//function redirectToLogin()     { window.location.href = '/Auth/Login'; }
//function redirectToDashboard() { window.location.href = '/Dashboard'; }

//// ── requireAuth: gọi ở đầu mọi trang cần đăng nhập ──────────────────────────
//function requireAuth() {
//    if (!Auth.isLoggedIn()) {
//        redirectToLogin();
//        return false;
//    }
//    _renderUserInfo();
//    _applyRoleVisibility();
//    return true;
//}

//// ── logout ────────────────────────────────────────────────────────────────────
//async function logout() {
//    try {
//        const refreshToken = Auth.getRefresh();
//        if (refreshToken) {
//            await fetch('/api/auth/logout', {
//                method: 'POST',
//                headers: {
//                    'Content-Type': 'application/json',
//                    'Authorization': 'Bearer ' + Auth.getToken()
//                },
//                body: JSON.stringify({ refreshToken })
//            });
//        }
//    } catch { /* bỏ qua lỗi mạng khi logout */ }
//    finally {
//        Auth.clear();
//        redirectToLogin();
//    }
//}

//// ── apiFetch: fetch wrapper tự gắn Bearer token ──────────────────────────────
//async function apiFetch(url, options = {}) {
//    const token = Auth.getToken();
//    options.headers = {
//        'Content-Type': 'application/json',
//        ...(token ? { 'Authorization': 'Bearer ' + token } : {}),
//        ...(options.headers || {})
//    };
//    const res = await fetch(url, options);
//    if (res.status === 401) {
//        Auth.clear();
//        redirectToLogin();
//    }
//    return res;
//}

//// ── Private: điền tên user vào sidebar ───────────────────────────────────────
//function _renderUserInfo() {
//    const nameEl   = document.getElementById('sidebar-user-name');
//    const avatarEl = document.getElementById('sidebar-user-avatar');
//    const roleEl   = document.getElementById('sidebar-user-role');
//    if (!nameEl) return; // trang NoShell (login) không có sidebar

//    const name  = Auth.getName();
//    const roles = Auth.getRoles();

//    if (nameEl)   nameEl.textContent   = name || 'Người dùng';
//    if (avatarEl) avatarEl.textContent = name ? name.charAt(0).toUpperCase() : '?';
//    if (roleEl)   roleEl.textContent   = roles.join(', ');
//}

//// ── Private: ẩn các phần tử cần role cụ thể ──────────────────────────────────
//function _applyRoleVisibility() {
//    document.querySelectorAll('[data-require-role]').forEach(el => {
//        const required = el.dataset.requireRole;
//        if (!Auth.hasRole(required)) el.style.display = 'none';
//    });
//}

//// ── Auto-requireAuth cho mọi trang KHÔNG phải NoShell ────────────────────────
//(function () {
//    // Trang login/forgot-password không có sidebar → không cần auth check
//    const noAuthPaths = ['/auth/login', '/auth/forgotpassword', '/auth/changepassword'];
//    const currentPath = window.location.pathname.toLowerCase();
//    const isPublicPage = noAuthPaths.some(p => currentPath.startsWith(p));

//    if (!isPublicPage) {
//        requireAuth();
//    }
//})();
