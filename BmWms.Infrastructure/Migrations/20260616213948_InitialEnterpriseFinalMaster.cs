using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmWms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialEnterpriseFinalMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetOtps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetOtps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfMeasure",
                columns: table => new
                {
                    UomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UomCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UomName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeasure", x => x.UomID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinThreshold = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AttributesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "SupplierContacts",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierContacts", x => x.ContactID);
                    table.ForeignKey(
                        name: "FK_SupplierContacts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseID);
                    table.ForeignKey(
                        name: "FK_Warehouses_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBarcodes",
                columns: table => new
                {
                    BarcodeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBarcodes", x => x.BarcodeID);
                    table.ForeignKey(
                        name: "FK_ProductBarcodes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProducts",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ContractPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProducts", x => new { x.SupplierID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_SupplierProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierProducts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UomConversions",
                columns: table => new
                {
                    ConversionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    FromUomID = table.Column<int>(type: "int", nullable: false),
                    ToUomID = table.Column<int>(type: "int", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UomConversions", x => x.ConversionID);
                    table.ForeignKey(
                        name: "FK_UomConversions_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UomConversions_UnitsOfMeasure_FromUomID",
                        column: x => x.FromUomID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UomID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UomConversions_UnitsOfMeasure_ToUomID",
                        column: x => x.ToUomID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsIssues",
                columns: table => new
                {
                    GIN_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GIN_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsIssues", x => x.GIN_ID);
                    table.ForeignKey(
                        name: "FK_GoodsIssues_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsIssues_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceipts",
                columns: table => new
                {
                    GRN_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRN_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceipts", x => x.GRN_ID);
                    table.ForeignKey(
                        name: "FK_GoodsReceipts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceipts_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReceipts_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventorySnapshots",
                columns: table => new
                {
                    SnapshotID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    StoredQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySnapshots", x => x.SnapshotID);
                    table.ForeignKey(
                        name: "FK_InventorySnapshots_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventorySnapshots_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockCounts",
                columns: table => new
                {
                    CountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCounts", x => x.CountID);
                    table.ForeignKey(
                        name: "FK_StockCounts_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockCounts_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                columns: table => new
                {
                    TransferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceWarehouseID = table.Column<int>(type: "int", nullable: false),
                    DestWarehouseID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.TransferID);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_DestWarehouseID",
                        column: x => x.DestWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_SourceWarehouseID",
                        column: x => x.SourceWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageLocations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    LocationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentLocationID = table.Column<int>(type: "int", nullable: true),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageLocations", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK_StorageLocations_StorageLocations_ParentLocationID",
                        column: x => x.ParentLocationID,
                        principalTable: "StorageLocations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageLocations_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsIssueDetails",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GIN_ID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UomID = table.Column<int>(type: "int", nullable: false),
                    GoodsIssueGIN_ID = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasureUomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsIssueDetails", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_GoodsIssueDetails_GoodsIssues_GoodsIssueGIN_ID",
                        column: x => x.GoodsIssueGIN_ID,
                        principalTable: "GoodsIssues",
                        principalColumn: "GIN_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsIssueDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsIssueDetails_UnitsOfMeasure_UnitOfMeasureUomID",
                        column: x => x.UnitOfMeasureUomID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptDetails",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRN_ID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UomID = table.Column<int>(type: "int", nullable: false),
                    GoodsReceiptGRN_ID = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasureUomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptDetails", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptDetails_GoodsReceipts_GoodsReceiptGRN_ID",
                        column: x => x.GoodsReceiptGRN_ID,
                        principalTable: "GoodsReceipts",
                        principalColumn: "GRN_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptDetails_UnitsOfMeasure_UnitOfMeasureUomID",
                        column: x => x.UnitOfMeasureUomID,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "UomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockCountDetails",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SystemQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CountedQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StockCountCountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCountDetails", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_StockCountDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockCountDetails_StockCounts_StockCountCountID",
                        column: x => x.StockCountCountID,
                        principalTable: "StockCounts",
                        principalColumn: "CountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferDetails",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StockTransferTransferID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferDetails", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_StockTransferDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransferDetails_StockTransfers_StockTransferTransferID",
                        column: x => x.StockTransferTransferID,
                        principalTable: "StockTransfers",
                        principalColumn: "TransferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBalances",
                columns: table => new
                {
                    BalanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    PhysicalQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AvailableQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CommittedQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StorageLocationLocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBalances", x => x.BalanceID);
                    table.ForeignKey(
                        name: "FK_InventoryBalances_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryBalances_StorageLocations_StorageLocationLocationID",
                        column: x => x.StorageLocationLocationID,
                        principalTable: "StorageLocations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeltaQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StorageLocationLocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_StorageLocations_StorageLocationLocationID",
                        column: x => x.StorageLocationLocationID,
                        principalTable: "StorageLocations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryCode",
                table: "Categories",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsIssueDetails_GoodsIssueGIN_ID",
                table: "GoodsIssueDetails",
                column: "GoodsIssueGIN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsIssueDetails_ProductID",
                table: "GoodsIssueDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsIssueDetails_UnitOfMeasureUomID",
                table: "GoodsIssueDetails",
                column: "UnitOfMeasureUomID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsIssues_CreatedBy",
                table: "GoodsIssues",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsIssues_WarehouseID",
                table: "GoodsIssues",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptDetails_GoodsReceiptGRN_ID",
                table: "GoodsReceiptDetails",
                column: "GoodsReceiptGRN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptDetails_ProductID",
                table: "GoodsReceiptDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptDetails_UnitOfMeasureUomID",
                table: "GoodsReceiptDetails",
                column: "UnitOfMeasureUomID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_CreatedBy",
                table: "GoodsReceipts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_SupplierID",
                table: "GoodsReceipts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_WarehouseID",
                table: "GoodsReceipts",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_ProductID_LocationID",
                table: "InventoryBalances",
                columns: new[] { "ProductID", "LocationID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_StorageLocationLocationID",
                table: "InventoryBalances",
                column: "StorageLocationLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySnapshots_ProductID",
                table: "InventorySnapshots",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySnapshots_WarehouseID",
                table: "InventorySnapshots",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductID",
                table: "InventoryTransactions",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ReferenceNumber",
                table: "InventoryTransactions",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_StorageLocationLocationID",
                table: "InventoryTransactions",
                column: "StorageLocationLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetOtps_Email",
                table: "PasswordResetOtps",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBarcodes_ProductID",
                table: "ProductBarcodes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserID",
                table: "RefreshTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleCode",
                table: "Roles",
                column: "RoleCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCountDetails_ProductID",
                table: "StockCountDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StockCountDetails_StockCountCountID",
                table: "StockCountDetails",
                column: "StockCountCountID");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_CreatedBy",
                table: "StockCounts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_WarehouseID",
                table: "StockCounts",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferDetails_ProductID",
                table: "StockTransferDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferDetails_StockTransferTransferID",
                table: "StockTransferDetails",
                column: "StockTransferTransferID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CreatedBy",
                table: "StockTransfers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_DestWarehouseID",
                table: "StockTransfers",
                column: "DestWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_SourceWarehouseID",
                table: "StockTransfers",
                column: "SourceWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocations_ParentLocationID",
                table: "StorageLocations",
                column: "ParentLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocations_WarehouseID",
                table: "StorageLocations",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierContacts_SupplierID",
                table: "SupplierContacts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_ProductID",
                table: "SupplierProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_UomCode",
                table: "UnitsOfMeasure",
                column: "UomCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UomConversions_FromUomID",
                table: "UomConversions",
                column: "FromUomID");

            migrationBuilder.CreateIndex(
                name: "IX_UomConversions_ProductID",
                table: "UomConversions",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_UomConversions_ToUomID",
                table: "UomConversions",
                column: "ToUomID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleID",
                table: "UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CreatedBy",
                table: "Warehouses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_WarehouseCode",
                table: "Warehouses",
                column: "WarehouseCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsIssueDetails");

            migrationBuilder.DropTable(
                name: "GoodsReceiptDetails");

            migrationBuilder.DropTable(
                name: "InventoryBalances");

            migrationBuilder.DropTable(
                name: "InventorySnapshots");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "PasswordResetOtps");

            migrationBuilder.DropTable(
                name: "ProductBarcodes");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "StockCountDetails");

            migrationBuilder.DropTable(
                name: "StockTransferDetails");

            migrationBuilder.DropTable(
                name: "SupplierContacts");

            migrationBuilder.DropTable(
                name: "SupplierProducts");

            migrationBuilder.DropTable(
                name: "UomConversions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "GoodsIssues");

            migrationBuilder.DropTable(
                name: "GoodsReceipts");

            migrationBuilder.DropTable(
                name: "StorageLocations");

            migrationBuilder.DropTable(
                name: "StockCounts");

            migrationBuilder.DropTable(
                name: "StockTransfers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UnitsOfMeasure");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
