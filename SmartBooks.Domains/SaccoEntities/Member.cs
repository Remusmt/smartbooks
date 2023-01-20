using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.SaccoEntities
{
    public class Member : SubLedgerBase
    {
        public Member()
        {
            MemberAttachments = new HashSet<MemberAttachment>();
            MemberApprovals = new HashSet<MemberApproval>();
            NextOfKins = new HashSet<NextOfKin>();
            MemberAccounts = new HashSet<MemberAccount>();
        }
        [StringLength(150)]
        public string MemberNumber { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
        public decimal Shared { get; set; }
        public MemberStatus MemberStatus { get; set; }
        public int ApplicationUserId { get; set; }
        [StringLength(150)]
        public string IndentificationNo { get; set; }
        public int? HomeAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        [StringLength(150)]
        public string NearestTown { get; set; }
        [StringLength(150)]
        public string Occupation { get; set; }
        public OccupationType OccupationType { get; set; }
        public LearntAboutUs LearntAboutUs { get; set; }
        public decimal SharesContribution { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Address HomeAddress { get; set; }
        public Address PermanentAddress { get; set; }

        public ICollection<MemberAttachment> MemberAttachments { get; set; }
        public ICollection<MemberApproval> MemberApprovals { get; set; }
        public ICollection<NextOfKin> NextOfKins { get; set; }
        public ICollection<MemberAccount> MemberAccounts { get; set; }
    }
}
