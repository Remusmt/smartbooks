using Microsoft.EntityFrameworkCore;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class JournalRepository<T> : Repository<T>, IJournalRepository<T>
        where T : Journal
    {
        public JournalRepository(SmartBooksContext context) : base(context)
        {
        }

        public async Task<Journal> GetDetailedByIdAsync(int id)
        {
            return await smartBooksContext.Set<T>()
                .Include(e => e.JournalDetails)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public bool ReferenceNumberExists(string referenceNumber, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.ReferenceNumber == referenceNumber && e.CompanyId == companyId 
                    && !e.IsDeleted);
        }
    }
}
