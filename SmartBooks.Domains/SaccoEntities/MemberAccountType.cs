using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.SaccoEntities
{
    public class MemberAccountType : TransactionItem
    {
        public int LedgerAccountId { get; set; }
        public PaymentItemType PaymentItemType { get; set; }
        /// <summary>
        /// Specifies the minimum a amount one can borrow for loans,
        /// minimum contribution for savings and shares,
        /// and default amount for fees
        /// </summary>
        public decimal Amount { get; set; }

        public LedgerAccount LedgerAccount { get; set; }
    }
}
