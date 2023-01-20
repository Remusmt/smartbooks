using SmartBooks.Domains.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class Company : BaseEntity
    {
        public DateTimeOffset CreatedOn { get; set; }
        public CompanyType CompanyType { get; set; }
        [StringLength(150)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string PoBox { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string Town { get; set; }
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }

        public Country Country { get; set; }
        public Currency Currency { get; set; }
    }
}
