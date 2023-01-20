using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.SchoolEntities
{
    public class SchoolTerm : AppBaseEntity
    {
        public int SchoolYearId { get; set; }
        public string Description { get; set; }

        public SchoolYear SchoolYear { get; set; }
    }
}
