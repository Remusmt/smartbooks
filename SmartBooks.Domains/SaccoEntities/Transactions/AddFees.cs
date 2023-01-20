using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.SaccoEntities.Transactions
{
    public class AddFees : Transaction
    {

    }

    public class FeeItem : TransactionDetail
    {
        public int MemberAccountId { get; set; }

        public MemberAccount MemberAccount { get; set; }
    }
}
