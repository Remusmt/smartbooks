using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;

namespace SmartBooks.Domains.Entities
{
    public class Journal : AppBaseEntity
    {
        public Journal()
        {
            JournalDetails = new HashSet<JournalDetail>();
        }

        public string ReferenceNumber { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Memo { get; set; }
        public decimal DebitSubTotal { get; set; }
        public decimal CreditSubTotal { get; set; }
        public decimal DebitTotalTax { get; set; }
        public decimal CreditTotalTax { get; set; }
        public decimal DebitTotal { get; set; }
        public decimal CreditTotal { get; set; }

        public int? TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }

        public ICollection<JournalDetail> JournalDetails { get; set; }
    }
}
