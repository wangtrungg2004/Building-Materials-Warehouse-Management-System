namespace BmWms.Core.Entities
{
    public class SupplierProduct
    {
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public decimal ContractPrice { get; set; }
    }
}