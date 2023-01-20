using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Persitence.Data.Context;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class MemberAccountRepository<T> : Repository<T>, IMemberAccountRepository<T>
        where T : MemberAccount
    {
        public MemberAccountRepository(SmartBooksContext context) : base(context)
        {
        }
    }
}
