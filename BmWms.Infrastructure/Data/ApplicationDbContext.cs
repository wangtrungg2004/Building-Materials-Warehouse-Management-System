using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryBalance> InventoryBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình Khóa chính phức hợp cho bảng trung gian phân quyền
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            // SỬA ĐỔI TẠI ĐÂY: Chỉ định rõ BalanceID là Khóa chính để EF Core thông suốt định tuyến
            modelBuilder.Entity<InventoryBalance>()
                .HasKey(ib => ib.BalanceID);

            // Khóa chặt đường dẫn để triệt tiêu lỗi Multiple Cascade Paths của SQL Server
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Ép các trường mã định danh phải là Duy nhất (Unique Constraints)
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleCode).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.ProductCode).IsUnique();
            modelBuilder.Entity<InventoryBalance>().HasIndex(ib => new { ib.ProductID, ib.StorageLocationCode }).IsUnique();

            // 3. Khóa ngoại tự tham chiếu bảo vệ danh tính người tạo tài khoản
            modelBuilder.Entity<User>()
                .HasOne(u => u.Creator)
                .WithMany()
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Khóa cứng định dạng decimal(18,4) chống lỗi làm tròn trữ lượng vật tư xây dựng
            modelBuilder.Entity<Product>().Property(p => p.MinThreshold).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.PhysicalQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.AvailableQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.CommittedQty).HasColumnType("decimal(18,4)");
        }
    }
}