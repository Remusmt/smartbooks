using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class TaxSpecification : BaseSpecification<Tax>
    {
        public TaxSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.TaxRates);
            ApplyOrderBy(e => e.Description);
        }
    }

    public class TaxRateSpecification : BaseSpecification<TaxRate>
    {
        public TaxRateSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.Tax);
            ApplyOrderBy(e => e.Description);
        }
    }
}
