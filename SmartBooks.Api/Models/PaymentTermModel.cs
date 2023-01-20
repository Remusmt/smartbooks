namespace SmartBooks.Api.Models
{
    public class PaymentTermModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool DateDriven { get; set; }
        public int NetDueIn { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int DiscountIfPaidWithin { get; set; }
        public int NetDueBefore { get; set; }
        public int DueNextMonthIfIssued { get; set; }
        public int DiscountIfPaidBefore { get; set; }
    }
}
