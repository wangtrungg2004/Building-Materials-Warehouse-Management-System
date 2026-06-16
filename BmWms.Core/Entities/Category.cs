using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryCode { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int? ParentCategoryID { get; set; }

        // Navigation Properties
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}