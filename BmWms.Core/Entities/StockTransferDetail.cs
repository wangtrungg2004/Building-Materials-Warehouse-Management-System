namespace BmWms.Core.Entities
{
    public class StockTransferDetail
    {
        public int DetailID { get; set; }
        public int TransferID { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }

        // Navigation Properties
        public StockTransfer StockTransfer { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}