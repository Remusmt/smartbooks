using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class BankSpecification : BaseSpecification<Bank>
    {
        public BankSpecification(int countryId) :
            base(e => e.IsDeleted == false && e.CountryId == countryId)
        {
            AddInclude(e => e.BankBranches);
            ApplyOrderBy(e => e.BankCode);
        }
    }
}
