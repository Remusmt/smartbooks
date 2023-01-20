using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class SchoolBlockSpecification<T> : BaseSpecification<T>
        where T : Block
    {
        public SchoolBlockSpecification(int companyId, bool detailed = false) :
            base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
            if (detailed)
            {
                AddInclude(e => e.ClassRooms);
            }
        }
    }
}
