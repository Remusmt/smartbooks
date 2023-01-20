using System;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// For double entry accounting. The amount is always positive
    /// Having debit and credit accounts in the same entry cuts the entries by half,
    /// it also ensures the entries are always balanced.
    /// </summary>
    public class LedgerEntry : AppBaseEntity
    {
        /// <summary>
        /// This is the transaction date
        /// holds date of when the transaction actually happened
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public int JournalId { get; set; }
        public int JournalDetailId { get; set; }
        public int DebitAccountId { get; set; }
        public int CreditAccountId { get; set; }
        /// <summary>
        /// For credits it will be negative and positive for debits
        /// Accounts balance will be sum of amount for account in a specific period
        /// </summary>
        public decimal Amount { get; set; }
        public bool Posted { get; set; }

        public LedgerAccount DebitAccount { get; set; }
        public LedgerAccount CreditAccount { get; set; }
        public Journal Journal { get; set; }
        public JournalDetail JournalDetail { get; set; }

    }
}
