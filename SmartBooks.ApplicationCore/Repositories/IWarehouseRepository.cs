using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IWarehouseRepository<T> : IRepository<T>
        where T: Warehouse
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        string GetCode(int companyId);
    }
}
