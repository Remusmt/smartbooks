using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IMemberAccountRepository<T> : IRepository<T>
        where T : MemberAccount
    {

    }
}
