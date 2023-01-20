using System.Collections.Generic;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// Holds transaction items, from this we will move to the generalledger
    /// This enables the ledger to have alot of info from the relation in this table
    /// which in turn relates to transactions
    /// </summary>
    public class TransactionDetail : AppBaseEntity //Rename to transaction detail
    {
        public TransactionDetail()
        {
            GeneralLedgers = new HashSet<GeneralLedger>();
        }
        public int TransactionId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int? CostCenterId { get; set; }
        public int? TransactionItemId { get; set; }

        public Transaction Transaction { get; set; }
        public CostCenter CostCenter { get; set; }
        public TransactionItem TransactionItem { get; set; }
        public ICollection<GeneralLedger> GeneralLedgers { get; set; }
    }
}
