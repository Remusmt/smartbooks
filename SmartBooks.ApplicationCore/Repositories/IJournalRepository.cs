using SmartBooks.Domains.Entities;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IJournalRepository<T> : IRepository<T>
        where T : Journal
    {
        bool ReferenceNumberExists(string referenceNumber, int companyId);
        public Task<Journal> GetDetailedByIdAsync(int id);
    }
}
