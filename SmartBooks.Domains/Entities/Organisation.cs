using SmartBooks.Domains.Enums;
using System.Collections.Generic;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// This class serves as the base class for customers and suppliers.
    /// </summary>
    public class Organisation : SubLedgerBase
    {
        public Organisation()
        {
            OrganisationAddresses = new HashSet<OrganisationAddress>();
        }
        public string VatNumber { get; set; }
        public int? DefaultAddressId { get; set; }
        public int? CategoryId { get; set; }
        public int? PaymentTermId { get; set; }
        public decimal CreditLimit { get; set; }
        public OrganisationType OrganisationType { get; set; }
        /// <summary>
        /// Period in days
        /// </summary>
        public int CreditLimitPeriod { get; set; }


        public OrganisationAddress DefaultAddress { get; set; }
        public Category Category { get; set; }
        public PaymentTerm PaymentTerm { get; set; }

        public ICollection<OrganisationAddress> OrganisationAddresses { get; set; }
    }
}
