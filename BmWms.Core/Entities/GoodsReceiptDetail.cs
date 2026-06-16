namespace BmWms.Core.Entities
{
    public class GoodsReceiptDetail
    {
        public int DetailID { get; set; }
        public int GRN_ID { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public int UomID { get; set; } // Đơn vị tính lúc bốc hàng (e.g., Tấn)

        // Navigation Properties
        public GoodsReceipt GoodsReceipt { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
    }
}