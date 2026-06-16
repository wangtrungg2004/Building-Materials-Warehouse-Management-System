using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int? CategoryID { get; set; }
        public string? Description { get; set; }
        public decimal MinThreshold { get; set; }
        public bool IsActive { get; set; } = true;
        public string? AttributesJson { get; set; } // Định dạng chuỗi JSON phẳng lưu thuộc tính động (Mác thép, đường kính)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Category? Category { get; set; }
        public ICollection<ProductBarcode> ProductBarcodes { get; set; } = new List<ProductBarcode>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}