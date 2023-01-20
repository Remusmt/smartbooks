using Microsoft.EntityFrameworkCore;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class TaxRepository<T> : Repository<T>, ITaxRepository<T>
        where T : Tax
    {
        public TaxRepository(SmartBooksContext context) : base(context)
        {
        }

        public bool CodeExists(string code, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Code == code && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DescriptionExists(string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Description == description && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DuplicateDescription(int id, string description, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.Description == description && e.CompanyId == companyId
                  && e.Id != id && !e.IsDeleted);
        }

        public bool DuplicateCode(int id, string code, int companyId)
        {
            return smartBooksContext.Set<T>()
               .Any(e => e.Code == code && e.CompanyId == companyId
               && e.Id != id && !e.IsDeleted);
        }

        public async Task<Tax> GetTaxFromTaxRateAsync(int id)
        {
           TaxRate taxRate = await smartBooksContext.TaxRates
                .Include(e => e.Tax)
                .FirstOrDefaultAsync(e => e.Id == id);
            return taxRate.Tax;
        }
    }
}
