using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class InventoryItemRepository<T> : Repository<T>, IInventoryItemRepository<T>
        where T : InventoryItem
    {
        public InventoryItemRepository(SmartBooksContext context) : base(context)
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

        public string GetCode(int companyId)
        {
            int lastId = 0;
            lastId = smartBooksContext.Set<T>()
                .Count(e => e.CompanyId == companyId);
            return (lastId + 1).ToString().PadLeft(4, '0');
        }
    }
}
