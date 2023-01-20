namespace SmartBooks.Domains.Entities
{
    public class JournalDetail: AppBaseEntity
    {
        public int JournalId { get; set; }
        public int LedgerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal TaxAmount { get; set; }
        public string Memo { get; set; }
        public int Sequence { get; set; }
        public int? SubLedgerId { get; set; }
        public int? TransactionId { get; set; }
        public int? TransactionDetailId { get; set; }
        public int? CostCenterId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? TaxRateId { get; set; }


        public Journal Journal { get; set; }
        public Transaction Transaction { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public LedgerAccount LedgerAccount { get; set; }
        public Organisation Organisation { get; set; }
        public CostCenter CostCenter { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public TaxRate TaxRate { get; set; }
    }
}
