using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IInventoryItemRepository<T> : IRepository<T>
        where T : InventoryItem
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        string GetCode(int companyId);
    }
}
