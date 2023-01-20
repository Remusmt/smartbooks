using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class SubLedgerBase : AppBaseEntity
    {
        /// <summary>
        /// This is the currency by which this org's balance is maintained
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Used when user wants to track an organisations transactions on the
        /// General ledger separately
        /// </summary>
        public int? LedgerAccountId { get; set; }
        /// <summary>
        /// Amount owing or owed. Saved in the org's currency
        /// </summary>
        /// 
        public decimal Balance { get; set; }
        public decimal CurrencyBalance { get; set; }

        /// <summary>
        /// Serves same purpose as code, not mandatory. 
        /// but neccessary for imports and should be unique
        /// </summary>
        [StringLength(150)]
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Name of the organisation
        /// </summary>
        [StringLength(150)]
        public string Name { get; set; }
        /// <summary>
        /// Contact person firstname
        /// </summary>
        [StringLength(150)]
        public string FirstName { get; set; }
        [StringLength(150)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string IdNumber { get; set; }
        public string PIN { get; set; }

        public Currency Currency { get; set; }
        public LedgerAccount LedgerAccount { get; set; }
    }
}
