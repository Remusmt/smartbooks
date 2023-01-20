using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class Bank : BaseEntity
    {
        public Bank()
        {
            BankBranches = new HashSet<BankBranch>();
        }
        public int CountryId { get; set; }
        [StringLength(50)]
        public string BankCode { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }

        public Country Country { get; set; }
        public ICollection<BankBranch> BankBranches { get; set; }

    }
}
