namespace BmWms.Core.Entities
{
    public class UnitOfMeasure
    {
        public int UomID { get; set; }
        public string UomCode { get; set; } = null!; // 'TON', 'CAY', 'BAO'
        public string UomName { get; set; } = null!;
    }
}