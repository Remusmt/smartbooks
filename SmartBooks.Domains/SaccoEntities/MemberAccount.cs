using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.SaccoEntities
{
    /// <summary>
    /// Holds details about all member accounts from shares, savings and loans
    /// </summary>
    public class MemberAccount: AppBaseEntity
    {
        public DateTime OpeningDate { get; set; }
        /// <summary>
        /// System generated from sacco product settings
        /// If added by user skip auto generation
        /// </summary>
        [StringLength(150)]
        public string AccountNumber { get; set; }
        public int MemberId { get; set; }
        public int MemberAccountTypeId { get; set; }
        public MemberAccountStatus AccountStatus { get; set; }
        /// <summary>
        /// Holds balance for this account
        /// </summary>
        public decimal Balance { get; set; }


        public Member Member { get; set; }
        public MemberAccountType MemberAccountType { get; set; }
    }
}
