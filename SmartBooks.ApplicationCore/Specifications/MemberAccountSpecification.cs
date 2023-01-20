using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class MemberAccountSpecification : BaseSpecification<MemberAccount>
    {
        public MemberAccountSpecification(int companyId) 
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.Member);
            AddInclude(e => e.MemberAccountType);
        }
    }
}
