using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // =============================================================================
        // CỔNG MẠNG THỰC THỂ (DBSET MAPPING) CHUẨN ĐỒ ÁN ENTERPRISE
        // =============================================================================
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<UomConversion> UomConversions { get; set; }
        public DbSet<ProductBarcode> ProductBarcodes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<InventoryBalance> InventoryBalances { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventorySnapshot> InventorySnapshots { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierContact> SupplierContacts { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        public DbSet<GoodsIssue> GoodsIssues { get; set; }
        public DbSet<GoodsIssueDetail> GoodsIssueDetails { get; set; }
        public DbSet<StockCount> StockCounts { get; set; }
        public DbSet<StockCountDetail> StockCountDetails { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferDetail> StockTransferDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─────────────────────────────────────────────────────────────────────────────
            // 1. ĐỊNH VỊ TƯỜNG MINH TOÀN BỘ KHÓA CHÍNH (PRIMARY KEYS MAPPING)
            // ─────────────────────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleID);
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserID, ur.RoleID });
            modelBuilder.Entity<SupplierProduct>().HasKey(sp => new { sp.SupplierID, sp.ProductID });
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryID);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
            modelBuilder.Entity<Warehouse>().HasKey(w => w.WarehouseID);
            modelBuilder.Entity<Supplier>().HasKey(s => s.SupplierID);
            modelBuilder.Entity<UnitOfMeasure>().HasKey(u => u.UomID);
            modelBuilder.Entity<StorageLocation>().HasKey(sl => sl.LocationID);
            modelBuilder.Entity<InventoryBalance>().HasKey(ib => ib.BalanceID);
            modelBuilder.Entity<UomConversion>().HasKey(uc => uc.ConversionID);
            modelBuilder.Entity<ProductBarcode>().HasKey(pb => pb.BarcodeID);
            modelBuilder.Entity<ProductImage>().HasKey(pi => pi.ImageID);
            modelBuilder.Entity<SupplierContact>().HasKey(sc => sc.ContactID);
            modelBuilder.Entity<GoodsReceipt>().HasKey(gr => gr.GRN_ID);
            modelBuilder.Entity<GoodsReceiptDetail>().HasKey(grd => grd.DetailID);
            modelBuilder.Entity<GoodsIssue>().HasKey(gi => gi.GIN_ID);
            modelBuilder.Entity<GoodsIssueDetail>().HasKey(gid => gid.DetailID);
            modelBuilder.Entity<StockCount>().HasKey(sc => sc.CountID);
            modelBuilder.Entity<StockCountDetail>().HasKey(scd => scd.DetailID);
            modelBuilder.Entity<StockTransfer>().HasKey(st => st.TransferID);
            modelBuilder.Entity<StockTransferDetail>().HasKey(std => std.DetailID);
            modelBuilder.Entity<InventoryTransaction>().HasKey(it => it.TransactionID);
            modelBuilder.Entity<InventorySnapshot>().HasKey(isn => isn.SnapshotID);

            // ─────────────────────────────────────────────────────────────────────────────
            // 2. MA TRẬN KHÓA CHẶT LUỒNG XÓA DÂY CHUYỀN (ANTI-CASCADE PATHS CONFIGURATION)
            // ─────────────────────────────────────────────────────────────────────────────
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Creator).WithMany().HasForeignKey(u => u.CreatedBy).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StorageLocation>()
                .HasOne(sl => sl.ParentLocation).WithMany(sl => sl.SubLocations).HasForeignKey(sl => sl.ParentLocationID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(st => st.SourceWarehouse).WithMany().HasForeignKey(st => st.SourceWarehouseID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(st => st.DestWarehouse).WithMany().HasForeignKey(st => st.DestWarehouseID).OnDelete(DeleteBehavior.Restrict);

            // SỬA ĐỔI CHÍ MẠNG TẠI ĐÂY: Khóa chặn Restrict cho hai cổng đơn vị tính From-To của UomConversions
            modelBuilder.Entity<UomConversion>()
                .HasOne(uc => uc.FromUom).WithMany().HasForeignKey(uc => uc.FromUomID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UomConversion>()
                .HasOne(uc => uc.ToUom).WithMany().HasForeignKey(uc => uc.ToUomID).OnDelete(DeleteBehavior.Restrict);

            // SỬA ĐỔI BỔ SUNG: Khóa chặt liên kết của Warehouse trỏ về User, triệt tiêu cột ẩn CreatorUserID tự sinh
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Creator).WithMany().HasForeignKey(w => w.CreatedBy).OnDelete(DeleteBehavior.Restrict);

            // Khóa an toàn bổ sung cho cụm tài khoản tạo chứng từ giao dịch
            modelBuilder.Entity<GoodsReceipt>().HasOne(gr => gr.Creator).WithMany().HasForeignKey(gr => gr.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GoodsIssue>().HasOne(gi => gi.Creator).WithMany().HasForeignKey(gi => gi.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockCount>().HasOne(sc => sc.Creator).WithMany().HasForeignKey(sc => sc.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockTransfer>().HasOne(st => st.Creator).WithMany().HasForeignKey(st => st.CreatedBy).OnDelete(DeleteBehavior.Restrict);

            // ─────────────────────────────────────────────────────────────────────────────
            // 3. ĐỒNG BỘ CẤU HÌNH ĐẶC THÙ RIÊNG CỦA PHÂN HỆ AUTHENTICATION ĐÃ CHẠY ỔN ĐỊNH
            // ─────────────────────────────────────────────────────────────────────────────
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.Property(x => x.TokenHash).HasMaxLength(200).IsRequired();
                e.Property(x => x.ReplacedByTokenHash).HasMaxLength(200);
                e.HasIndex(x => x.TokenHash).IsUnique();
                e.Ignore(x => x.IsActive);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PasswordResetOtp>(e =>
            {
                e.Property(x => x.Email).HasMaxLength(100).IsRequired();
                e.Property(x => x.Otp).HasMaxLength(10).IsRequired();
                e.HasIndex(x => x.Email);
            });

            // ─────────────────────────────────────────────────────────────────────────────
            // 4. KHÓA CỨNG ĐỘ CHÍNH XÁC decimal(18,4) CHỐNG SAI SỐ KHI QUY ĐỔI VLXD
            // ─────────────────────────────────────────────────────────────────────────────
            modelBuilder.Entity<SupplierProduct>().Property(sp => sp.ContractPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().Property(p => p.MinThreshold).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<UomConversion>().Property(uc => uc.ConversionFactor).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.PhysicalQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.AvailableQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryBalance>().Property(ib => ib.CommittedQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<GoodsReceiptDetail>().Property(grd => grd.Quantity).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<GoodsIssueDetail>().Property(gid => gid.Quantity).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StockCountDetail>().Property(scd => scd.SystemQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StockCountDetail>().Property(scd => scd.CountedQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StockTransferDetail>().Property(std => std.Quantity).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryTransaction>().Property(it => it.DeltaQty).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventorySnapshot>().Property(isn => isn.StoredQty).HasColumnType("decimal(18,4)");

            // ─────────────────────────────────────────────────────────────────────────────
            // 5. THIẾT LẬP UNIQUE INDEXES ĐỒNG BỘ TOÀN VẸN VỚI MASTER SCRIPT VẬT LÝ
            // ─────────────────────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleCode).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.ProductCode).IsUnique();
            modelBuilder.Entity<Warehouse>().HasIndex(w => w.WarehouseCode).IsUnique();
            modelBuilder.Entity<UnitOfMeasure>().HasIndex(u => u.UomCode).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryCode).IsUnique();
            modelBuilder.Entity<InventoryBalance>().HasIndex(ib => new { ib.ProductID, ib.LocationID }).IsUnique();
            modelBuilder.Entity<InventoryTransaction>().HasIndex(it => it.ReferenceNumber);
        }
    }
}