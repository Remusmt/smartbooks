using SmartBooks.Domains.Enums;

namespace SmartBooks.Api.Models
{
    public class LedgerAccountModel
    {
        public int Id { get; set; }
        public int UpdateCode { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public int CurrencyId { get; set; }
        public int? ParentAccountId { get; set; }
        public int? TaxRateId { get; set; }
        public decimal Balance { get; set; }
        public decimal CurrencyBalance { get; set; }
        public DetailAccountType DetailAccountType { get; set; }
        public string BankAccountNo { get; set; }
        public bool HasOverDraft { get; set; }
        public decimal OverDraftLimit { get; set; }
    }
}
