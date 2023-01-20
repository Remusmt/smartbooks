using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class LedgerAccountSpecification : BaseSpecification<LedgerAccount>
    {
        public LedgerAccountSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            ApplyOrderByDescending(e => e.Height);
            
        }
    }
}
