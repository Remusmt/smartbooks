using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.SaccoEntities
{
    public class SavingsItem : SaccoProduct
    {
        public SavingsItem()
        {
            PaymentItemType = Enums.PaymentItemType.Savings;
        }
        /// <summary>
        /// Member can specify who they are saving for.
        /// Defaults to members name if nothing is specified
        /// </summary>
        [StringLength(150)]
        public string BeneficiaryName { get; set; }
        [StringLength(150)]
        public string BeneficiaryPhoneNo { get; set; }
    }
}
