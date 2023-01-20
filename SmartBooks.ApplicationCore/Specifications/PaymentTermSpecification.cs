using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class PaymentTermSpecification : BaseSpecification<PaymentTerm>
    {
        public PaymentTermSpecification(int companyId) 
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            ApplyOrderBy(e => e.Description);
        }
    }
}
