namespace BmWms.Core.Entities
{
    public class ProductImage
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImageUrl { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}