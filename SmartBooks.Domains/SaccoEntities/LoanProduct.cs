using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.SaccoEntities
{
    /// <summary>
    /// Defines a loan, it inherits from sacco accounts
    /// </summary>
    public class LoanProduct: SaccoProduct
    {
        //public int PenaltyAccountId { get; set; }

        /// <summary>
        /// Income account for posting to general ledger penaties from this loan
        /// </summary>
        //public LedgerAccount PenaltyAccount { get; set; }

    }
}
