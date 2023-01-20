namespace SmartBooks.Api.Models
{
    public class TaxAgencyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public int? DefaultAddressId { get; set; }
        public int? CategoryId { get; set; }
        public int? PaymentTermId { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditLimitPeriod { get; set; }
        public int? LedgerAccountId { get; set; }
    }
}
