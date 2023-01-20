using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class UnitofMeasureSpecification : BaseSpecification<UnitofMeasure>
    {
        public UnitofMeasureSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            ApplyOrderBy(e => e.Description);
        }
    }
}
