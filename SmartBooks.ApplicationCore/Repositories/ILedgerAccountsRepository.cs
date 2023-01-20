using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface ILedgerAccountsRepository<T> : IRepository<T>
        where T : LedgerAccount
    {
        bool AccountNameExists(string name, int companyId);
        bool DuplicateAccountName(int id, string name, int companyId);
        bool AccountNumberExists(string accountNumber, int companyId);
        bool DuplicateAccountNumber(int id, string accountNumber, int companyId);
    }
}
