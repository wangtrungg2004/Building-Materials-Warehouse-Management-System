namespace BmWms.Core.Entities
{
    public class StockCountDetail
    {
        public int DetailID { get; set; }
        public int CountID { get; set; }
        public int ProductID { get; set; }
        public decimal SystemQty { get; set; }  // Số lượng đóng băng trên hệ thống lúc bắt đầu kiểm kê
        public decimal CountedQty { get; set; } // Số lượng thực tế nhân viên đi đếm cân hàng ngoài bãi bãi

        // Navigation Properties
        public StockCount StockCount { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}