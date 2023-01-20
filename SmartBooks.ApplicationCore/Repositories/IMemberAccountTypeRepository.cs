using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IMemberAccountTypeRepository<T> : IRepository<T>
        where T : MemberAccountType
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        string GetCode(int companyId, int maxLength);
    }
}
