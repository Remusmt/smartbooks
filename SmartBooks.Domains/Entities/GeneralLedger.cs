using System;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// There will be a collection on the transaction_item
    /// All transactions must have details and after evaluating the items
    /// the system generates the required ledger entries
    /// </summary>
    public class GeneralLedger : AppBaseEntity
    {
        /// <summary>
        /// This is the transaction date
        /// holds date of when the transaction actually happened
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public int JournalId { get; set; }
        public int JournalDetailId { get; set; }
        public int LedgerAccountId { get; set; }
        /// <summary>
        /// For credits it will be negative and positive for debits
        /// Accounts balance will be sum of amount for account in a specific period
        /// </summary>
        public decimal Amount { get; set; }
        public Journal Journal { get; set; }
        public JournalDetail JournalDetail { get; set; }
        public LedgerAccount LedgerAccount { get; set; }
    }
}
