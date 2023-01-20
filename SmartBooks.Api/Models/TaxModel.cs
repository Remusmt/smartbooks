using SmartBooks.Domains.Enums;

namespace SmartBooks.Api.Models
{
    public class TaxModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string TaxRegistrationNumber { get; set; }
        public int TaxAgencyId { get; set; }
        public ReportingMethod ReportingMethod { get; set; }
        public int PurchasesAccountId { get; set; }
        public int SalesAccountId { get; set; }
    }
}
