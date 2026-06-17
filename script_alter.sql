-- =====================================================
-- ALTER TABLE SCRIPT FOR PRODUCT ATTRIBUTES
-- Database: BuildMatWMS_DB
-- Date: 2026-06-17
-- =====================================================

-- Check if tables exist, if not create them
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductAttributes')
BEGIN
    PRINT 'Creating ProductAttributes table...'
    
    CREATE TABLE [dbo].[ProductAttributes](
        [AttributeID] [int] IDENTITY(1,1) NOT NULL,
        [AttributeCode] [nvarchar](50) NOT NULL,
        [AttributeName] [nvarchar](100) NOT NULL,
        [DataType] [nvarchar](20) NOT NULL,
        [Options] [nvarchar](500) NULL,
        [IsRequired] [bit] NOT NULL DEFAULT(0),
        [DisplayOrder] [int] NOT NULL DEFAULT(0),
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [CreatedAt] [datetime] NOT NULL DEFAULT(getdate()),
        [UpdatedAt] [datetime] NULL,
        CONSTRAINT [PK_ProductAttributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC)
    )
    
    CREATE UNIQUE NONCLUSTERED INDEX [UK_AttributeCode] ON [dbo].[ProductAttributes]([AttributeCode] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductAttributeValues')
BEGIN
    PRINT 'Creating ProductAttributeValues table...'
    
    CREATE TABLE [dbo].[ProductAttributeValues](
        [ValueID] [int] IDENTITY(1,1) NOT NULL,
        [ProductID] [int] NOT NULL,
        [AttributeID] [int] NOT NULL,
        [TextValue] [nvarchar](500) NULL,
        [NumberValue] [decimal](18,4) NULL,
        [BoolValue] [bit] NOT NULL DEFAULT(0),
        [DateValue] [datetime2](7) NULL,
        [CreatedAt] [datetime] NOT NULL DEFAULT(getdate()),
        CONSTRAINT [PK_ProductAttributeValues] PRIMARY KEY CLUSTERED ([ValueID] ASC)
    )
    
    CREATE UNIQUE NONCLUSTERED INDEX [UK_ProductAttribute] ON [dbo].[ProductAttributeValues]([ProductID] ASC, [AttributeID] ASC)
    
    -- Add Foreign Keys
    ALTER TABLE [dbo].[ProductAttributeValues] WITH CHECK 
        ADD CONSTRAINT [FK_PAV_Products] FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID]) ON DELETE CASCADE
    ALTER TABLE [dbo].[ProductAttributeValues] CHECK CONSTRAINT [FK_PAV_Products]
    
    ALTER TABLE [dbo].[ProductAttributeValues] WITH CHECK 
        ADD CONSTRAINT [FK_PAV_ProductAttributes] FOREIGN KEY([AttributeID]) REFERENCES [dbo].[ProductAttributes] ([AttributeID]) ON DELETE CASCADE
    ALTER TABLE [dbo].[ProductAttributeValues] CHECK CONSTRAINT [FK_PAV_ProductAttributes]
END
GO

-- Fix Foreign Key for Users self-reference
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Users_CreatedBy')
BEGIN
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users_CreatedBy]
END
GO
ALTER TABLE [dbo].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Users_CreatedBy] 
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE RESTRICT
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_CreatedBy]
GO

PRINT 'ALTER TABLE completed successfully!'
GO

-- =====================================================
-- SEED DATA FOR CONSTRUCTION MATERIALS INDUSTRY
-- Product Attributes
-- =====================================================

PRINT 'Seeding Product Attributes for Construction Industry...'

-- Clear existing data (optional - comment out if you want to keep existing data)
-- DELETE FROM [ProductAttributeValues]
-- DELETE FROM [ProductAttributes]
-- DBCC CHECKIDENT ('ProductAttributes', RESEED, 0)
-- DBCC CHECKIDENT ('ProductAttributeValues', RESEED, 0)

-- =====================================================
-- CEMENT & CONCRETE ATTRIBUTES
-- =====================================================

INSERT INTO [ProductAttributes] ([AttributeCode], [AttributeName], [DataType], [Options], [IsRequired], [DisplayOrder], [IsActive], [CreatedAt])
VALUES 
-- Cement Type (Dropdown)
('CEMENT_TYPE', 'Loại Xi Măng', 'Dropdown', 'PCB30,PCB40,PCB50,Xi măng hỗn hợp,Xi măng white,Xi măng poóc lăng', 1, 1, 1, GETDATE()),

-- Cement Brand (Dropdown)
('CEMENT_BRAND', 'Thương Hiệu Xi Măng', 'Dropdown', 'Hà Tiên,Holcim,Lafarge,Insee,Việt Úc,Fico,Bim Son,Hải Phòng', 1, 2, 1, GETDATE()),

-- Cement Weight (Number - kg)
('CEMENT_WEIGHT', 'Trọng Lượng (kg)', 'Number', NULL, 1, 3, 1, GETDATE()),

-- Cement Standard (Dropdown)
('CEMENT_STANDARD', 'Tiêu Chuẩn', 'Dropdown', 'TCVN 2682:2009,TCVN 6260:2009,TCVN 7711:2007,ASTM C150,EN 197-1', 1, 4, 1, GETDATE()),

-- Cement Color (Dropdown)
('CEMENT_COLOR', 'Màu Sắc', 'Dropdown', 'Xám đen,Trắng,Hồng', 0, 5, 1, GETDATE()),

-- Cement Has Additive (Boolean)
('CEMENT_HAS_ADDITIVE', 'Có Phụ Gia', 'Boolean', NULL, 0, 6, 1, GETDATE()),

-- =====================================================
-- STEEL & IRON ATTRIBUTES
-- =====================================================

('STEEL_TYPE', 'Loại Thép', 'Dropdown', 'Thép thanh,Thép cuộn,Thép hình,Thép tấm,Thép ống,Thép dây', 1, 10, 1, GETDATE()),

-- Steel Grade (Dropdown)
('STEEL_GRADE', 'Mác Thép', 'Dropdown', 'CB240-T,CB300-V,CB400-V,CB500-V,CB600-V,Grade 250,Grade 350,Grade 415,Grade 500', 1, 11, 1, GETDATE()),

-- Steel Diameter (Number - mm)
('STEEL_DIAMETER', 'Đường Kính (mm)', 'Number', NULL, 1, 12, 1, GETDATE()),

-- Steel Origin (Dropdown)
('STEEL_ORIGIN', 'Xuất Xứ', 'Dropdown', 'Việt Nam,Trung Quốc,Nhật Bản,Hàn Quốc,Đài Loan,Ấn Độ,Nga', 1, 13, 1, GETDATE()),

-- Steel Standard (Dropdown)
('STEEL_STANDARD', 'Tiêu Chuẩn', 'Dropdown', 'TCVN 1651-1:2018,TCVN 1651-2:2018,TCVN 1654:2008,JIS G3112,ASTM A615,BS 4449', 1, 14, 1, GETDATE()),

-- Steel Surface (Dropdown)
('STEEL_SURFACE', 'Bề Mặt', 'Dropdown', 'Trơn,Gân,Vằn,L gai', 0, 15, 1, GETDATE()),

-- Steel IsRebar (Boolean)
('STEEL_IS_REBAR', 'Thép Cốt Bê Tông', 'Boolean', NULL, 0, 16, 1, GETDATE()),

-- =====================================================
-- SAND & AGGREGATE ATTRIBUTES
-- =====================================================

('SAND_TYPE', 'Loại Cát', 'Dropdown', 'Cát vàng,Cát đen,Cát nghiền,Cát tự nhiên,Cát nhân tạo', 1, 20, 1, GETDATE()),

-- Sand Size (Dropdown)
('SAND_SIZE', 'Kích Thước Hạt', 'Dropdown', 'Mịn (0.14-0.3mm),Trung (0.3-0.6mm),Thô (0.6-1.25mm),To (>1.25mm)', 1, 21, 1, GETDATE()),

-- Sand Source (Dropdown)
('SAND_SOURCE', 'Nguồn Gốc', 'Dropdown', 'Bình Định,Quảng Nam,Quảng Ngãi,Đà Nẵng,Khánh Hòa,Bình Thuận', 0, 22, 1, GETDATE()),

-- Sand FM (Decimal - Fineness Modulus)
('SAND_FM', 'Mô Đun Độ Lớn (FM)', 'Decimal', NULL, 1, 23, 1, GETDATE()),

-- Sand Clay Content (Decimal - %)
('SAND_CLAY', 'Hàm Lượng Bùn (%)', 'Decimal', NULL, 1, 24, 1, GETDATE()),

-- Sand Meets Standard (Boolean)
('SAND_MEETS_STD', 'Đạt Tiêu Chuẩn', 'Boolean', NULL, 1, 25, 1, GETDATE()),

-- =====================================================
-- STONE & GRAVEL ATTRIBUTES
-- =====================================================

('STONE_TYPE', 'Loại Đá', 'Dropdown', 'Đá 0x4,Đá 1x2,Đá 2x4,Đá 4x6,Đá 5x7,Đá dăm,Đá san lấp', 1, 30, 1, GETDATE()),

-- Stone Size (Dropdown)
('STONE_SIZE', 'Kích Thước (mm)', 'Dropdown', '5-10,10-20,20-40,40-70,70-150', 1, 31, 1, GETDATE()),

-- Stone Origin (Dropdown)
('STONE_ORIGIN', 'Mỏ Đá', 'Dropdown', 'Khe Giao,Bình Tường,Thạch Nhất,Núi Pháo,Hà Nam,Bình Dương', 0, 32, 1, GETDATE()),

-- Stone Compressive Strength (Number - MPa)
('STONE_STRENGTH', 'Cường Độ Chịu Nén (MPa)', 'Number', NULL, 1, 33, 1, GETDATE()),

-- =====================================================
-- BRICK & BLOCK ATTRIBUTES
-- =====================================================

('BRICK_TYPE', 'Loại Gạch', 'Dropdown', 'Gạch đất nung,Gạch block,Gạch không nung,Gạch silite,Gạch ống,Gạch xi măng cốt liệu', 1, 40, 1, GETDATE()),

-- Brick Size (Dropdown)
('BRICK_SIZE', 'Kích Thước', 'Dropdown', '220x105x65mm,210x100x60mm,200x95x55mm,380x180x180mm,390x190x190mm,390x100x200mm', 1, 41, 1, GETDATE()),

-- Brick Compressive Strength (Number - kg/cm²)
('BRICK_STRENGTH', 'Cường Độ Chịu Nén (kg/cm²)', 'Number', NULL, 1, 42, 1, GETDATE()),

-- Brick Standard (Dropdown)
('BRICK_STANDARD', 'Tiêu Chuẩn', 'Dropdown', 'TCVN 1450:2009,TCVN 6477:2016,TCVN 6260:2009', 0, 43, 1, GETDATE()),

-- Brick Color (Dropdown)
('BRICK_COLOR', 'Màu Sắc', 'Dropdown', 'Đỏ cam,Nâu,Hồng,Xám,Nâu đỏ', 0, 44, 1, GETDATE()),

-- =====================================================
-- TILE & FLOORING ATTRIBUTES
-- =====================================================

('TILE_TYPE', 'Loại Gạch Men', 'Dropdown', 'Gạch lát nền,Gạch ốp tường,Gạch granite,Gạch ceramic,Gạch mosaic,Gạch terrazzo', 1, 50, 1, GETDATE()),

-- Tile Size (Text)
('TILE_SIZE', 'Kích Thước', 'Text', NULL, 1, 51, 1, GETDATE()),

-- Tile Color (Dropdown)
('TILE_COLOR', 'Màu Sắc', 'Dropdown', 'Trắng,Kem,Vàng,Nâu,Xám,Đen,Đỏ,Xanh lá,Beige,Miami,Carara', 1, 52, 1, GETDATE()),

-- Tile Surface (Dropdown)
('TILE_SURFACE', 'Bề Mặt', 'Dropdown', 'Bóng mờ,Bóng láng,Mờ,Nhám,Răng cưa,Họa tiết', 0, 53, 1, GETDATE()),

-- Tile Water Absorption (Decimal - %)
('TILE_ABSORPTION', 'Độ Hút Nước (%)', 'Decimal', NULL, 1, 54, 1, GETDATE()),

-- Tile Brand (Dropdown)
('TILE_BRAND', 'Thương Hiệu', 'Dropdown', 'Viglacera,Taicera,Prime,Bitexco,Đồng Tâm,CMC,MISA,Hospital', 1, 55, 1, GETDATE()),

-- =====================================================
-- PAINT & COATING ATTRIBUTES
-- =====================================================

('PAINT_TYPE', 'Loại Sơn', 'Dropdown', 'Sơn nước,Sơn dầu,Sơn epoxy,Sơn chống gỉ,Sơn lót,Sơn trang trí', 1, 60, 1, GETDATE()),

-- Paint Color (Text)
('PAINT_COLOR', 'Mã Màu', 'Text', NULL, 1, 61, 1, GETDATE()),

-- Paint Finish (Dropdown)
('PAINT_FINISH', 'Bề Mặt Hoàn Thiện', 'Dropdown', 'Bóng,Mờ,Bán bóng,Siêu bóng,Satin,Metalic', 0, 62, 1, GETDATE()),

-- Paint Coverage (Number - m²/L)
('PAINT_COVERAGE', 'Độ Phủ (m²/L)', 'Number', NULL, 1, 63, 1, GETDATE()),

-- Paint Volume (Number - L)
('PAINT_VOLUME', 'Thể Tích (Lít)', 'Number', NULL, 1, 64, 1, GETDATE()),

-- Paint Has AntiMold (Boolean)
('PAINT_ANTI_MOLD', 'Chống Nấm Mốc', 'Boolean', NULL, 0, 65, 1, GETDATE()),

-- Paint Has UV (Boolean)
('PAINT_ANTI_UV', 'Chống Tia UV', 'Boolean', NULL, 0, 66, 1, GETDATE()),

-- =====================================================
-- WOOD & TIMBER ATTRIBUTES
-- =====================================================

('WOOD_TYPE', 'Loại Gỗ', 'Dropdown', 'Gỗ tự nhiên,Gỗ công nghiệp,Gỗ ghép thanh,Gỗ MDF,Gỗ HDF,Gỗ Plywood,Gỗ HDF', 1, 70, 1, GETDATE()),

-- Wood Species (Dropdown)
('WOOD_SPECIES', 'Loài Gỗ', 'Dropdown', 'Keo,Bạch đàn,Cao su,Tếch,Mahogany (Gỗ đỏ),Oak (Sồi),Walnut (Óc chó),Ash (Tần bì)', 0, 71, 1, GETDATE()),

-- Wood Thickness (Number - mm)
('WOOD_THICKNESS', 'Độ Dày (mm)', 'Number', NULL, 1, 72, 1, GETDATE()),

-- Wood Width (Number - mm)
('WOOD_WIDTH', 'Chiều Rộng (mm)', 'Number', NULL, 1, 73, 1, GETDATE()),

-- Wood Length (Number - mm)
('WOOD_LENGTH', 'Chiều Dài (mm)', 'Number', NULL, 1, 74, 1, GETDATE()),

-- Wood Has FSC (Boolean)
('WOOD_HAS_FSC', 'Chứng Nhận FSC', 'Boolean', NULL, 0, 75, 1, GETDATE()),

-- =====================================================
-- PLUMBING & PIPING ATTRIBUTES
-- =====================================================

('PIPE_TYPE', 'Loại Ống', 'Dropdown', 'Ống PVC,Ống PPR,Ống HDPE,Ống PE,Ống đồng,Ống inox,Ống nhựa mềm', 1, 80, 1, GETDATE()),

-- Pipe Diameter (Number - mm)
('PIPE_DIAMETER', 'Đường Kính (mm)', 'Number', NULL, 1, 81, 1, GETDATE()),

-- Pipe Pressure (Number - Bar)
('PIPE_PRESSURE', 'Áp Suất Làm Việc (Bar)', 'Number', NULL, 1, 82, 1, GETDATE()),

-- Pipe Wall Thickness (Decimal - mm)
('PIPE_WALL', 'Độ Dày Thành (mm)', 'Decimal', NULL, 1, 83, 1, GETDATE()),

-- Pipe Standard (Dropdown)
('PIPE_STANDARD', 'Tiêu Chuẩn', 'Dropdown', 'TCVN,Kí hiệu JIS,Kí hiệu DIN,ISO 1452,AS/NZS 1477', 0, 84, 1, GETDATE()),

-- Pipe Color (Dropdown)
('PIPE_COLOR', 'Màu Ống', 'Dropdown', 'Trắng,Xanh,Xám,Đen,Vàng,Nâu', 0, 85, 1, GETDATE()),

-- =====================================================
-- ELECTRICAL ATTRIBUTES
-- =====================================================

('ELECTRIC_TYPE', 'Loại Vật Tư', 'Dropdown', 'Dây điện,Cáp điện,Ổ cắm,Công tắc,Hộp điện,CB,MCB,MCCB,Đèn LED,Đèn huỳnh quang', 1, 90, 1, GETDATE()),

-- Electric Voltage (Dropdown)
('ELECTRIC_VOLTAGE', 'Điện Áp (V)', 'Dropdown', '220V,380V,12V,24V,110V,12-24V', 1, 91, 1, GETDATE()),

-- Electric Current (Number - A)
('ELECTRIC_CURRENT', 'Cường Độ Dòng Điện (A)', 'Number', NULL, 0, 92, 1, GETDATE()),

-- Electric Section (Number - mm²)
('ELECTRIC_SECTION', 'Tiết Diện (mm²)', 'Number', NULL, 0, 93, 1, GETDATE()),

-- Electric Standard (Dropdown)
('ELECTRIC_STANDARD', 'Tiêu Chuẩn', 'Dropdown', 'TCVN 3743,TCVN 6613,IEC 60227,IEC 60502,BS 6004', 0, 94, 1, GETDATE()),

-- Electric Brand (Dropdown)
('ELECTRIC_BRAND', 'Thương Hiệu', 'Dropdown', 'Cadivi,Daphaco,Thiên Phú,LS,Vina,Panasonic,Sino,Lioa', 1, 95, 1, GETDATE()),

-- =====================================================
-- ROOFING & WATERPROOFING ATTRIBUTES
-- =====================================================

('ROOF_TYPE', 'Loại Tôn', 'Dropdown', 'Tôn lợp,Tôn cách nhiệt,Tôn mạ màu,Tôn giả ngói,Tấm lợp Polycarbonate,Mái tôn composite', 1, 100, 1, GETDATE()),

-- Roof Thickness (Decimal - mm)
('ROOF_THICKNESS', 'Độ Dày (mm)', 'Decimal', NULL, 1, 101, 1, GETDATE()),

-- Roof Color (Dropdown)
('ROOF_COLOR', 'Màu Sắc', 'Dropdown', 'Xanh ngọc,Đỏ đậm,Nâu,Vàng be,Trắng,Ghi,Nâu cafe', 1, 102, 1, GETDATE()),

-- Roof Profile (Dropdown)
('ROOF_PROFILE', 'Sóng/Răng', 'Dropdown', '5 sóng,7 sóng,9 sóng,11 sóng,Sóng vuông,Sóng tròn,Sóng đặc,Sóng rỗng', 1, 103, 1, GETDATE()),

-- Roof Width (Number - mm)
('ROOF_WIDTH', 'Chiều Rộng Hữu Ích (mm)', 'Number', NULL, 1, 104, 1, GETDATE()),

-- =====================================================
-- CHEMICAL & ADDITIVE ATTRIBUTES
-- =====================================================

('CHEMICAL_TYPE', 'Loại Hóa Chất', 'Dropdown', 'Phụ gia bê tông,Keo dán gạch,Vữa mạch,Chất chống thấm,Sơn chống dột,Mỡ bôi trơn,Dung môi', 1, 110, 1, GETDATE()),

-- Chemical Package (Dropdown)
('CHEMICAL_PACKAGE', 'Quy Cách Đóng Gói', 'Dropdown', 'Kg,Túi 25kg,Thùng 20L,Thùng 5L,Can 5L,Lon 1L,Bịch 1kg', 1, 111, 1, GETDATE()),

-- Chemical Has MSDS (Boolean)
('CHEMICAL_HAS_MSDS', 'Có Phiếu An Toàn (MSDS)', 'Boolean', NULL, 1, 112, 1, GETDATE()),

-- Chemical Expiry Date (Date)
('CHEMICAL_EXPIRY', 'Hạn Sử Dụng', 'Date', NULL, 1, 113, 1, GETDATE()),

-- Chemical Hazard Class (Dropdown)
('CHEMICAL_HAZARD', 'Phân Loại Nguy Hiểm', 'Dropdown', 'Không nguy hiểm,Nguy hiểm nhẹ,Nguy hiểm trung bình,Nguy hiểm cao,Hóa chất ăn mòn,Hóa chất dễ cháy', 0, 114, 1, GETDATE()),

-- Chemical Origin (Dropdown)
('CHEMICAL_ORIGIN', 'Xuất Xứ', 'Dropdown', 'Việt Nam,Trung Quốc,Đức,Mỹ,Nhật Bản,Hàn Quốc,Thái Lan,Singapore', 0, 115, 1, GETDATE())

GO

PRINT 'Seed data inserted successfully!'
PRINT 'Total attributes: ' + CAST((SELECT COUNT(*) FROM ProductAttributes) AS VARCHAR)
GO
