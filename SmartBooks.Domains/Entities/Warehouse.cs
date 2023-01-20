using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class Warehouse : AppBaseEntity
    {
        public Warehouse()
        {
            Bins = new HashSet<Bin>();
        }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public int DefaultReceivingBin { get; set; }
        public int DefaultDespatchBin { get; set; }

        public ICollection<Bin> Bins { get; set; }
    }
}

       