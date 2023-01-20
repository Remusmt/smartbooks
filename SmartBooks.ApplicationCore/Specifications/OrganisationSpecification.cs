using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class OrganisationSpecification<T> : BaseSpecification<T>
        where T : Organisation
    {
        public OrganisationSpecification(int companyId) :
            base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.OrganisationAddresses);
            ApplyOrderBy(e => e.Name);
        }

        public OrganisationSpecification(int companyId, string sort, string order, int page, int pageSize)
           : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.OrganisationAddresses);
            ApplyPaging((page - 1) * pageSize, pageSize);
            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (order == "desc")
                {
                    switch (sort.ToLower())
                    {
                        case "name":
                            ApplyOrderByDescending(e => e.Name);
                            break;
                        case "balance":
                            ApplyOrderByDescending(e => e.Balance);
                            break;
                    }
                }
                else
                {
                    switch (sort.ToLower())
                    {
                        case "name":
                            ApplyOrderBy(e => e.Name);
                            break;
                        case "balance":
                            ApplyOrderBy(e => e.Balance);
                            break;
                    }
                }
            }
        }
    }
}
