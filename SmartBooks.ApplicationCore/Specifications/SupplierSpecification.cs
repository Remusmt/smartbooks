using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class SupplierSpecification: BaseSpecification<Supplier>
    {
        public SupplierSpecification(int companyId, bool isTaxAgency) :
        base(e => e.CompanyId == companyId && !e.IsDeleted && e.IsTaxAgency == isTaxAgency)
        {
            AddInclude(e => e.OrganisationAddresses);
            ApplyOrderBy(e => e.Name);
        }
    }
}
