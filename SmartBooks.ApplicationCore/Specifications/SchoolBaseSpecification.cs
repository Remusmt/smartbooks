using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class SchoolBaseSpecification<T> : BaseSpecification<T>
        where T : SchoolBaseClass
    {
        public SchoolBaseSpecification(int companyId) :
            base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
        }
    }
}
