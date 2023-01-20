using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IPaymentTermRepository<T> : IRepository<T>
        where T : PaymentTerm
    {
        bool DescriptionExists(string description, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
    }
}
