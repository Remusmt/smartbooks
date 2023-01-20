namespace SmartBooks.Api.Models
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DefaultReceivingBin { get; set; }
        public int DefaultDespatchBin { get; set; }
    }
}
