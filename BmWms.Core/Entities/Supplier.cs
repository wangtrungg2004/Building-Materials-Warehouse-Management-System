using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierCode { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public string TaxId { get; set; } = null!;

        // Navigation Properties
        public ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
        public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    }
}