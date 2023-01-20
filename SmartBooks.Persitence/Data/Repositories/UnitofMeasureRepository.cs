using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class UnitofMeasureRepository<T> : Repository<T>, IUnitofMeasureRepository<T>
        where T : UnitofMeasure
    {
        public UnitofMeasureRepository(SmartBooksContext context) : base(context)
        {
        }

        public bool AbbreviationExists(string abbreviation, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Abbreviation == abbreviation
                && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DescriptionExists(string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Description == description
                && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DuplicateAbbreviation(int id, string abbreviation, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.Abbreviation == abbreviation && e.CompanyId == companyId
                  && e.Id != id && !e.IsDeleted);
        }

        public bool DuplicateDescription(int id, string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.Description == description && e.CompanyId == companyId
                  && e.Id != id && !e.IsDeleted);
        }
    }
}
