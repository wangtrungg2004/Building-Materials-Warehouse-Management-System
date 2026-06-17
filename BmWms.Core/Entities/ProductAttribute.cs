using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    /// <summary>
    /// Thuộc tính động của sản phẩm
    /// Ví dụ: Màu sắc (Dropdown), Trọng lượng (Number), Có bảo hành (Boolean)
    /// </summary>
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeCode { get; set; } = null!;   // VD: "COLOR", "WEIGHT", "HAS_WARRANTY"
        public string AttributeName { get; set; } = null!;   // VD: "Màu Sắc", "Trọng Lượng"
        public string DataType { get; set; } = null!;        // "Text", "Number", "Decimal", "Boolean", "Dropdown", "Date"
        public string? Options { get; set; }                  // JSON array cho dropdown: ["Đỏ","Xanh","Vàng"]
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
    }

    /// <summary>
    /// Giá trị thuộc tính của từng sản phẩm cụ thể
    /// </summary>
    public class ProductAttributeValue
    {
        public int ValueID { get; set; }
        public int ProductID { get; set; }
        public int AttributeID { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public bool BoolValue { get; set; }
        public DateTime? DateValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Product Product { get; set; } = null!;
        public ProductAttribute Attribute { get; set; } = null!;
    }
}
