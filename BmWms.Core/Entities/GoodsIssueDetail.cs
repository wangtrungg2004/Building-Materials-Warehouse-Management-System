namespace BmWms.Core.Entities
{
    public class GoodsIssueDetail
    {
        public int DetailID { get; set; }
        public int GIN_ID { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public int UomID { get; set; }

        // Navigation Properties
        public GoodsIssue GoodsIssue { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
    }
}