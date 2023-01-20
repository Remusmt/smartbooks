namespace SmartBooks.Domains.SaccoEntities
{
    /// <summary>
    /// Can have only one of this and should be system generated
    /// </summary>
    public class SharesItem : MemberAccountType
    {
        public SharesItem()
        {
            PaymentItemType = Enums.PaymentItemType.Savings;
        }
    }
}
