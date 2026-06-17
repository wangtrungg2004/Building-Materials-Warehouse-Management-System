using System;

namespace BmWms.Core.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; } = null!;     // VD: "SP001", "VL-XIMANG-01"
        public string ProductName { get; set; } = null!;     // VD: "Xi măng PCB40"
        public string? Description { get; set; }

        // FK → ProductGroup
        public int ProductGroupID { get; set; }
        public ProductGroup ProductGroup { get; set; } = null!;

        // Đơn vị tính
        public string UnitOfMeasure { get; set; } = "Pcs";  // "Pcs", "Kg", "Bao", "Cây", "Tấm"...

        // Thông tin mở rộng
        public string? SKU { get; set; }                     // Stock Keeping Unit
        public string? Barcode { get; set; }
        public string? Brand { get; set; }                   // Thương hiệu: "Hà Tiên", "Hòa Phát"...
        public string? OriginCountry { get; set; }           // Xuất xứ: "Việt Nam", "Trung Quốc"...

        // Thuộc tính vật lý
        public decimal? Weight { get; set; }                 // Trọng lượng (kg)
        public decimal? DimensionLength { get; set; }        // Dài (cm)
        public decimal? DimensionWidth { get; set; }         // Rộng (cm)
        public decimal? DimensionHeight { get; set; }        // Cao (cm)
        public int? ShelfLife { get; set; }                  // Hạn sử dụng (ngày)
        public int? WarrantyPeriod { get; set; }             // Bảo hành (tháng)

        // Ảnh & tags
        public string? ImageUrl { get; set; }
        public string? Tags { get; set; }                    // Comma-separated tags

        // Ngưỡng tối thiểu tồn kho
        public decimal MinThreshold { get; set; }

        // SỬA TẠI ĐÂY: bit -> bool
        public bool IsActive { get; set; } = true;

        // Audit
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
    }
}