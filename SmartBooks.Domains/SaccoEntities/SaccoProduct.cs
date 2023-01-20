using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.SaccoEntities
{
    public class SaccoProduct : MemberAccountType
    {
        /// <summary>
        /// Specifies if member can have multiple accounts of this product
        /// </summary>
        public bool CanHaveMultiple { get; set; }
        /// <summary>
        /// Product interest rate specified per year
        /// </summary>
        public decimal InterestRate { get; set; }
        /// <summary>
        /// Defines how interest amount is computed
        /// </summary>
        public InterestType InterestType { get; set; }
        /// <summary>
        /// Defines the minimum consecutive shares contributions for one
        /// to qualify for this product
        /// </summary>
        public int MinConsecutiveContribution { get; set; }
        /// <summary>
        /// Default to true,
        /// If false ensure user enters a value
        /// Ensure is unique in company
        /// </summary>
        public bool AutoGenerateAccountNumbers { get; set; }
        /// <summary>
        /// Specifies the prefix for account numbers (reference number)
        /// </summary>
        [StringLength(150)]
        public string AccountNumberPrefix { get; set; }
        /// <summary>
        /// Specifies the length for the account number
        /// </summary>
        public int MaxAccountLength { get; set; }
        /// <summary>
        /// System incremented to specify next account number
        /// </summary>
        public int NextAccountNumber { get; set; }

        public int InterestAccountId { get; set; }

        public LedgerAccount InterestAccount { get; set; } 
    }
}
