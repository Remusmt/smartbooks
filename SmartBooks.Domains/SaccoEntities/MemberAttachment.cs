using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;

namespace SmartBooks.Domains.SaccoEntities
{
    public class MemberAttachment : AppBaseEntity
    {
        public MemberAttachment()
        {
            CreatedOn = DateTimeOffset.UtcNow;
        }
        public int MemberId { get; set; }
        public int AttachmentId { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public Member Member { get; set; }
        public Attachment Attachment { get; set; }
    }
}
