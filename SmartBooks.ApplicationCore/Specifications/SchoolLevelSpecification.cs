using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class SchoolLevelSpecification<T> : BaseSpecification<T>
        where T : Level
    {
        public SchoolLevelSpecification(int companyId, bool detailed = false) :
            base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
            if (detailed)
            {
                AddInclude(e => e.ClassRooms);
            }
        }
    }
}
