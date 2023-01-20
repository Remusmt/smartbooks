using SmartBooks.Domains.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class UnitofMeasure : AppBaseEntity
    {
        [StringLength(50)]
        public string Abbreviation { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public UomType Type { get; set; }
    }
}
