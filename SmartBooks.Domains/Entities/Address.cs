using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// Serves as the base class for address
    /// </summary>
    public class Address : BaseEntity
    {
        public string Location { get; set; }
        [StringLength(50)]
        public string PoBox { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
