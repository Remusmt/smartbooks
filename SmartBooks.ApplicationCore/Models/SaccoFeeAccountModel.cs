using System;

namespace SmartBooks.ApplicationCore.Models
{
    public class SaccoFeeAccountModel
    {
        public int CompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime OpeningDate { get; set; }
        public string AccountNumber { get; set; }
        public int MemberId { get; set; }
        public int MemberAccountTypeId { get; set; }
        public decimal OpeningBalance { get; set; }
    }
}
