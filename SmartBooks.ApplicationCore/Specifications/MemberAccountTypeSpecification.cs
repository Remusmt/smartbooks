using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class MemberAccountTypeSpecification<T> : BaseSpecification<T>
        where T : MemberAccountType
    {
        public MemberAccountTypeSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            ApplyOrderBy(e => e.Description);
        }
    }
}
