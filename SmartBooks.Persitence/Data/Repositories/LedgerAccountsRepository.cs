using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class LedgerAccountsRepository<T> : Repository<T>, ILedgerAccountsRepository<T>
        where T : LedgerAccount
    {
        public LedgerAccountsRepository(SmartBooksContext context) : base(context)
        {
        }

        public bool AccountNameExists(string name, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.AccountName == name
                && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool AccountNumberExists(string accountNumber, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.AccountNumber == accountNumber
                && e.CompanyId == companyId && !e.IsDeleted);
        }

        public bool DuplicateAccountName(int id, string name, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.AccountName == name && e.CompanyId == companyId
                  && e.Id != id && !e.IsDeleted);
        }

        public bool DuplicateAccountNumber(int id, string accountNumber, int companyId)
        {
            return smartBooksContext.Set<T>()
                 .Any(e => e.AccountNumber == accountNumber && e.CompanyId == companyId
                 && e.Id != id && !e.IsDeleted);
        }

    }
}
