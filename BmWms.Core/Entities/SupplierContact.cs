namespace BmWms.Core.Entities
{
    public class SupplierContact
    {
        public int ContactID { get; set; }
        public int SupplierID { get; set; }
        public string ContactName { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        // Navigation Property
        public Supplier Supplier { get; set; } = null!;
    }
}