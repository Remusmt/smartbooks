using SmartBooks.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface ICategoryRepository<T> : IRepository<T>
        where T : Category
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        string GetCode(int companyId);
    }
}
