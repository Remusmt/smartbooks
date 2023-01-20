using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class ClassRoomSpecification<T> : BaseSpecification<T>
        where T : ClassRoom
    {
        public ClassRoomSpecification(int companyId, bool detailed = false) :
            base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
            if (detailed)
            {
                AddInclude(e => e.Block);
                AddInclude(e => e.Level);
            }
        }
    }
}
