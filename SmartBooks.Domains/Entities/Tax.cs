using SmartBooks.Domains.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class Tax : AppBaseEntity
    {
        public Tax()
        {
            TaxRates = new HashSet<TaxRate>();
        }

        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public string TaxRegistrationNumber { get; set; }
        public int TaxAgencyId { get; set; }
        public ReportingMethod ReportingMethod { get; set; }
        public int? PurchasesAccountId { get; set; }
        public int? SalesAccountId { get; set; }


        public LedgerAccount PurchasesAccount { get; set; }
        public LedgerAccount SalesAccount { get; set; }
        public Supplier TaxAgency { get; set; }

        public ICollection<TaxRate> TaxRates  { get; set; }
    }
}
