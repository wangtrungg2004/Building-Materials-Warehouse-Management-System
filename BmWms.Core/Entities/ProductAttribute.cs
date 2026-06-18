using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    /// <summary>
    /// Định nghĩa thuộc tính của sản phẩm
    /// Ví dụ: Color, Brand, Material
    /// </summary>
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeCode { get; set; } = null!;
        public string AttributeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductAttributeValue> Values { get; set; } = new List<ProductAttributeValue>();
    }

    /// <summary>
    /// Giá trị của thuộc tính
    /// </summary>
    public class ProductAttributeValue
    {
        public int ValueID { get; set; }
        public int AttributeID { get; set; }
        public string ValueName { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ProductAttribute Attribute { get; set; } = null!;
        public ICollection<ProductAttributeSelection> Selections { get; set; } = new List<ProductAttributeSelection>();
    }

    /// <summary>
    /// Liên kết Product với ProductAttributeValue
    /// </summary>
    public class ProductAttributeSelection
    {
        public int SelectionID { get; set; }
        public int ProductID { get; set; }
        public int ValueID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; } = null!;
        public ProductAttributeValue Value { get; set; } = null!;
    }
}
