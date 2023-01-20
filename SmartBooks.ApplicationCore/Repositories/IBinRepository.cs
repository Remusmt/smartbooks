using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IBinRepository<T> : IRepository<T>
        where T : Bin
    {
        bool CodeExists(string code, int warehouseId);
        bool DescriptionExists(string description, int warehouseId);
        bool DuplicateCode(int id, string code, int warehouseId);
        bool DuplicateDescription(int id, string description, int warehouseId);
        string GetCode(int warehouseId);
    }
}
