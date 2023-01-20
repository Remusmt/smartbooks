using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class BinsSpecification : BaseSpecification<Bin>
    {
        public BinsSpecification(int warehouseId) :
            base(e => e.WarehouseId == warehouseId && e.IsDeleted == false)
        {
            AddInclude(e => e.Warehouse);
            ApplyOrderBy(e => e.Description);
        }
    }
}
