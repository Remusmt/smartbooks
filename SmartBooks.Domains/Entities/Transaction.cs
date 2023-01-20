using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// All transactions must inherit from this class
    /// All source documents must inherit this class
    /// this makes it easy to report from one table
    /// </summary>
    public class Transaction : AppBaseEntity
    {
        public Transaction()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
        }
        public DateTime TransactionDate { get; set; }
        public TransactionType DocumentType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public int SubLedgerBaseId { get; set; }
        public int? JournalId { get; set; }
        public string DocumentName { get; set; }
        //public AmountRates AmountInclusiveofTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal Total { get; set; }
        /// <summary>
        /// If transaction is sent to client/supplier
        /// Sent
        /// Pending
        /// </summary>
        public int EmailStatus { get; set; }
        public string EmailMessage { get; set; }
        public string EmailAddress { get; set; }
        public bool CanCreate()
        {
            return false;
        }

        public bool CanEdit()
        {
            return false;
        }

        public bool CanDelete()
        {
            return false;
        }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }

    }
}
