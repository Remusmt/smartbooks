using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class WarehouseSpecifications : BaseSpecification<Warehouse>
    {
        public WarehouseSpecifications(int companyId) :
            base(e => e.CompanyId == companyId && e.IsDeleted == false)
        {
            AddInclude(e => e.Bins);
            ApplyOrderBy(e => e.Description);
        }
    }
}
