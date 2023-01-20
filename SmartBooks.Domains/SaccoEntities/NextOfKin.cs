using SmartBooks.Domains.Entities;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.SaccoEntities
{
    public class NextOfKin : AppBaseEntity
    {
        public int MemberId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string Relation { get; set; }
        [StringLength(150)]
        public string Contacts { get; set; }
        public bool IsMinor { get; set; }
        [StringLength(150)]
        public string CareOf { get; set; }
        public decimal Percentage { get; set; }

        public Member Member { get; set; }
    }
}
