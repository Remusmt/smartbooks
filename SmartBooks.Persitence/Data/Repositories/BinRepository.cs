using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class BinRepository<T> : Repository<T>, IBinRepository<T>
        where T : Bin
    {
        public BinRepository(SmartBooksContext context) : base(context)
        {
        }

        public void Testd()
        {
            base.Test();
        }
        public bool CodeExists(string code, int warehouseId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Code == code && e.WarehouseId == warehouseId && !e.IsDeleted);
        }

        public bool DescriptionExists(string description, int warehouseId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Description == description && e.WarehouseId == warehouseId && !e.IsDeleted);
        }

        public bool DuplicateDescription(int id, string description, int warehouseId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.Description == description && e.WarehouseId == warehouseId 
                  && e.Id != id && !e.IsDeleted);
        }

        public bool DuplicateCode(int id, string code, int warehouseId)
        {
            return smartBooksContext.Set<T>()
               .Any(e => e.Code == code && e.WarehouseId == warehouseId 
               && e.Id != id && !e.IsDeleted);
        }

        public string GetCode(int warehouseId)
        {
            int lastId = 0;
            lastId = smartBooksContext.Set<T>()
                .Count(e => e.WarehouseId == warehouseId);
            return (lastId + 1).ToString().PadLeft(4, '0');
        }
    }
}
