using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.SaccoEntities
{
    public class SaccoSettings : AppBaseEntity
    {
        /// <summary>
        /// Sacco auto increment number
        /// </summary>
        public int NextMemberNumber { get; set; }
        /// <summary>
        /// Holds the shares item Id
        /// </summary>
        public int SharesAccountTypeId { get; set; }
        /// <summary>
        /// Holds Membership fee item Id
        /// </summary>
        public int MembershipFeeAccountTypeId { get; set; }
    }
}
