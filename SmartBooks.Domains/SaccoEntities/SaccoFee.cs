using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.SaccoEntities
{
    /// <summary>
    /// Defines fees levied to members
    /// System should create Membership fee Item by default
    /// </summary>
    public class SaccoFee : MemberAccountType
    {
        public SaccoFee()
        {
            PaymentItemType = PaymentItemType.Fees;
        }
        public SaccoFeesType SaccoFeesType { get; set; }
    }
}
