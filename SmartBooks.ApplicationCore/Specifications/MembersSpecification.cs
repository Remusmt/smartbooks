using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class MembersSpecification : BaseSpecification<Member>
    {
        public MembersSpecification(int companyId, bool detailed = false) :
          base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
            if (detailed)
            {
                AddInclude(e => e.NextOfKins);
                AddInclude(e => e.MemberApprovals);
                AddInclude(e => e.HomeAddress);
                AddInclude(e => e.PermanentAddress);
                AddInclude(e => e.MemberAttachments);
                AddInclude("MemberAttachments.Attachment");
            }
        }
    }
}
