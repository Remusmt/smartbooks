using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.SchoolEntities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class SchoolBaseRepository<T> : Repository<T>, ISchoolBaseRepository<T>
        where T : SchoolBaseClass
    {
        public SchoolBaseRepository(SmartBooksContext context) : base(context) { }
        public bool DescriptionExists(string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Description == description 
                && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DuplicateDescription(int id, string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.Description == description && e.CompanyId == companyId
                  && e.Id != id && !e.IsDeleted);
        }
    }
}
