using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.SaccoEntities
{
    public class MemberApproval : AppBaseEntity
    {
        public int ApplicationUserId { get; set; }
        public int MemberId { get; set; }
        public ApprovalAction ApprovalAction { get; set; }
        public string MessageToMember { get; set; }
        public string Comments { get; set; }

        public Member Member { get; set; }
    }
}
