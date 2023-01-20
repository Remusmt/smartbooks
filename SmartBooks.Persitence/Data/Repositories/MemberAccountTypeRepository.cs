using Microsoft.EntityFrameworkCore;
using SmartBooks.ApplicationCore.Helpers;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Persitence.Data.Context;
using SmartBooks.Persitence.Data.Helpers;
using System.Linq;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class MemberAccountTypeRepository<T> : Repository<T>, IMemberAccountTypeRepository<T>
        where T : MemberAccountType
    {
        public MemberAccountTypeRepository(SmartBooksContext context) : base(context)
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

        public string GetCode(int companyId, int maxLength)
        {
            SimpleValue simpleValue = smartBooksContext.SingleValue
                .FromSqlRaw($"SELECT MAX(Code) AS Value FROM TransactionItems " +
                $"WHERE CompanyId = {companyId} AND Discriminator = '{typeof(T).Name}' AND IsDeleted = 0")
                .FirstOrDefault();
            int maxId = simpleValue == null ? 0 : simpleValue.Value.GetNumberFromString();
            maxId++;
            string code = (maxId).ToString().PadLeft(maxLength, '0');
            bool accNameExist = true;
            while (accNameExist)
            {
                accNameExist = CodeExists(code, companyId);
                if (accNameExist)
                {
                    maxId += 1;
                    code = (maxId).ToString().PadLeft(maxLength, '0');
                }
            }
            return code;
        }
    }
}
