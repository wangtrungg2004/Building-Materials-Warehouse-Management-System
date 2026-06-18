using System;

namespace BmWms.Core.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string? Barcode { get; set; }
        public string Status { get; set; }

        public int? GroupID { get; set; }
        public ProductGroup Group { get; set; }

        public ICollection<ProductAttributeValue> AttributeValues { get; set; }
    }
}