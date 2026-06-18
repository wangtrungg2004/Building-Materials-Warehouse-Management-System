using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
        public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
        public DbSet<ProductAttributeValue> ProductAttributeValues => Set<ProductAttributeValue>();

        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<SupplierProduct> SupplierProducts => Set<SupplierProduct>();

        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();

        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

        public DbSet<InboundOrder> InboundOrders => Set<InboundOrder>();
        public DbSet<InboundOrderDetail> InboundOrderDetails => Set<InboundOrderDetail>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PasswordResetOtp> PasswordResetOtps => Set<PasswordResetOtp>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // USER
            // =========================
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.UserID);

                e.Property(x => x.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                e.HasIndex(x => x.Username).IsUnique();

                e.Property(x => x.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.RoleID);

                e.Property(x => x.RoleCode).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.RoleCode).IsUnique();

                e.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasKey(x => new { x.UserID, x.RoleID });

                e.HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // PRODUCT
            // =========================
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.ProductID);

                e.Property(x => x.ProductCode)
                    .HasMaxLength(50)
                    .IsRequired();

                e.HasIndex(x => x.ProductCode).IsUnique();

                e.Property(x => x.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");

                e.HasOne(x => x.Group)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.GroupID);
            });

            modelBuilder.Entity<ProductGroup>(e =>
            {
                e.HasKey(x => x.GroupID);

                e.Property(x => x.GroupName)
                    .HasMaxLength(150)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductAttribute>(e =>
            {
                e.HasKey(x => x.AttributeID);

                e.Property(x => x.AttributeName)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductAttributeValue>(e =>
            {
                e.HasKey(x => new { x.ProductID, x.AttributeID });

                e.Property(x => x.Value)
                    .HasMaxLength(255)
                    .IsRequired();

                e.HasOne(x => x.Product)
                    .WithMany(x => x.AttributeValues)
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Attribute)
                    .WithMany(x => x.Values)
                    .HasForeignKey(x => x.AttributeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // SUPPLIER
            // =========================
            modelBuilder.Entity<Supplier>(e =>
            {
                e.HasKey(x => x.SupplierID);

                e.Property(x => x.SupplierName)
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(x => x.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
            });

            modelBuilder.Entity<SupplierProduct>(e =>
            {
                e.HasKey(x => new { x.SupplierID, x.ProductID });

                e.HasOne(x => x.Supplier)
                    .WithMany(x => x.SupplierProducts)
                    .HasForeignKey(x => x.SupplierID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // WAREHOUSE
            // =========================
            modelBuilder.Entity<Warehouse>(e =>
            {
                e.HasKey(x => x.WarehouseID);

                e.Property(x => x.WarehouseCode)
                    .HasMaxLength(50)
                    .IsRequired();

                e.HasIndex(x => x.WarehouseCode).IsUnique();

                e.Property(x => x.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
            });

            modelBuilder.Entity<StorageLocation>(e =>
            {
                e.HasKey(x => x.LocationID);

                e.Property(x => x.LocationCode)
                    .HasMaxLength(50)
                    .IsRequired();

                e.HasIndex(x => x.LocationCode).IsUnique();

                e.HasOne(x => x.Warehouse)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.WarehouseID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Inventory>(e =>
            {
                e.HasKey(x => new { x.ProductID, x.WarehouseID });

                e.Property(x => x.Quantity).HasDefaultValue(0);
                e.Property(x => x.ReservedQuantity).HasDefaultValue(0);

                e.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InventoryTransaction>(e =>
            {
                e.HasKey(x => x.TransactionID);

                e.Property(x => x.Timestamp)
                    .HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.Inventory)
                    .WithMany()
                    .HasForeignKey(x => new { x.ProductID, x.WarehouseID })
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // INBOUND
            // =========================
            modelBuilder.Entity<InboundOrder>(e =>
            {
                e.HasKey(x => x.InboundNo);

                e.Property(x => x.InboundDate)
                    .HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.Supplier)
                    .WithMany()
                    .HasForeignKey(x => x.SupplierID);

                e.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy);
            });

            modelBuilder.Entity<InboundOrderDetail>(e =>
            {
                e.HasKey(x => new { x.InboundNo, x.ProductID });

                e.HasOne(x => x.InboundOrder)
                    .WithMany(x => x.Details)
                    .HasForeignKey(x => x.InboundNo)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductID);
            });

            // =========================
            // AUTH
            // =========================
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasIndex(x => x.TokenHash).IsUnique();

                e.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PasswordResetOtp>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                e.Property(x => x.IsUsed)
                    .HasDefaultValue(false);

                e.HasIndex(x => x.Email).IsUnique();
            });
        }
    }
}