using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.Entities
{
    public class Employee : SubLedgerBase
    {
        public Gender Gender { get; set; }
        public string NhifNumber { get; set; }
        public string NssfNumber { get; set; }
        public string PayslipPassword { get; set; }
        public string ItaxResidentialStatus { get; set; }
    }
}
