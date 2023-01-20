using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.HrEntities
{
    public class Designation : AppBaseEntity
    {
        public string Description { get; set; }
        public int? ReportsToId { get; set; }
        public Designation ReportsTo { get; set; }
    }
}
