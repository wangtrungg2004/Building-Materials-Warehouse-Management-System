namespace BmWms.Core.Entities
{
    public class ProductBarcode
    {
        public int BarcodeID { get; set; }
        public int ProductID { get; set; }
        public string Barcode { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}