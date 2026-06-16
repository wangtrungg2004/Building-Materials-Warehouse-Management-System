namespace BmWms.Core.Entities
{
    public class UomConversion
    {
        public int ConversionID { get; set; }
        public int ProductID { get; set; }
        public int FromUomID { get; set; }
        public int ToUomID { get; set; }
        public decimal ConversionFactor { get; set; }

        // Navigation Properties
        public Product Product { get; set; } = null!;
        public UnitOfMeasure FromUom { get; set; } = null!;
        public UnitOfMeasure ToUom { get; set; } = null!;
    }
}