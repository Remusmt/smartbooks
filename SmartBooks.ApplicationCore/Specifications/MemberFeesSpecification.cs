using SmartBooks.Domains.SaccoEntities.Transactions;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class MemberFeesSpecification : BaseSpecification<AddFees>
    {
        protected MemberFeesSpecification(int companyId) 
            : base(e => e.CompanyId == companyId &&  !e.IsDeleted)
        {
        }
    }
}
