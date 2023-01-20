using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class BankBranch : BaseEntity
    {
        public int BankId { get; set; }
        [StringLength(50)]
        public string BranchCode { get; set; }
        [StringLength(150)]
        public string BranchName { get; set; }
        [StringLength(50)]
        public string SwiftCode { get; set; }

        public Bank Bank { get; set; }

    }
}
