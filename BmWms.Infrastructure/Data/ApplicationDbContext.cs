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
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<InventoryBalance> InventoryBalances { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

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

            // ── ProductGroup ────────────────────────────────────────────────────────
            modelBuilder.Entity<ProductGroup>(e =>
            {
                e.HasKey(x => x.ProductGroupID);
                e.HasIndex(x => x.GroupCode).IsUnique();
                e.Property(x => x.GroupCode).HasMaxLength(50).IsRequired();
                e.Property(x => x.GroupName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.CreatedBy).HasMaxLength(100);
            });

            // ── Product (mở rộng) ───────────────────────────────────────────────────
            modelBuilder.Entity<Product>(e =>
            {
                e.Property(x => x.ProductCode).HasMaxLength(50).IsRequired();
                e.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.UnitOfMeasure).HasMaxLength(20).IsRequired();
                e.Property(x => x.SKU).HasMaxLength(50);
                e.Property(x => x.Barcode).HasMaxLength(50);
                e.Property(x => x.Brand).HasMaxLength(100);
                e.Property(x => x.OriginCountry).HasMaxLength(100);
                e.Property(x => x.ImageUrl).HasMaxLength(500);
                e.Property(x => x.Tags).HasMaxLength(500);
                e.Property(x => x.CreatedBy).HasMaxLength(100);
                e.Property(x => x.Weight).HasColumnType("decimal(18,4)");
                e.Property(x => x.DimensionLength).HasColumnType("decimal(18,4)");
                e.Property(x => x.DimensionWidth).HasColumnType("decimal(18,4)");
                e.Property(x => x.DimensionHeight).HasColumnType("decimal(18,4)");

                // FK → ProductGroup
                e.HasOne(x => x.ProductGroup)
                 .WithMany(g => g.Products)
                 .HasForeignKey(x => x.ProductGroupID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── ProductAttribute ───────────────────────────────────────────────────────
            modelBuilder.Entity<ProductAttribute>(e =>
            {
                e.HasKey(x => x.AttributeID);
                e.Property(x => x.AttributeCode).HasMaxLength(50).IsRequired();
                e.Property(x => x.AttributeName).HasMaxLength(100).IsRequired();
                e.Property(x => x.DataType).HasMaxLength(20).IsRequired();
                e.HasIndex(x => x.AttributeCode).IsUnique();
            });

            // ── ProductAttributeValue ─────────────────────────────────────────────────
            modelBuilder.Entity<ProductAttributeValue>(e =>
            {
                e.HasKey(x => x.ValueID);
                e.HasIndex(x => new { x.ProductID, x.AttributeID }).IsUnique();

                e.HasOne(x => x.Product)
                 .WithMany()
                 .HasForeignKey(x => x.ProductID)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Attribute)
                 .WithMany(a => a.ProductAttributeValues)
                 .HasForeignKey(x => x.AttributeID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── RefreshTokens ─────────────────────────────────────────────────────────
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.TokenHash).HasMaxLength(200).IsRequired();
                e.Property(x => x.ReplacedByTokenHash).HasMaxLength(200);
                e.HasIndex(x => x.TokenHash).IsUnique();
                e.Ignore(x => x.IsActive); // computed property, không map vào cột DB

                e.HasOne(x => x.User).WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── PasswordResetOtps ──────────────────────────────────────────────────────
            modelBuilder.Entity<PasswordResetOtp>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Email).HasMaxLength(100).IsRequired();
                e.Property(x => x.Otp).HasMaxLength(10).IsRequired();
                e.HasIndex(x => x.Email);
            });
        }
    }
}