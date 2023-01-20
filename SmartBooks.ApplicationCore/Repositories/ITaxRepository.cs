using SmartBooks.Domains.Entities;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface ITaxRepository<T> : IRepository<T>
        where T : Tax
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        Task<Tax> GetTaxFromTaxRateAsync(int id);
    }
}
