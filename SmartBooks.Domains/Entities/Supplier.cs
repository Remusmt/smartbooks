namespace SmartBooks.Domains.Entities
{
    public class Supplier : Organisation
    {
        public int? BankId { get; set; }
        public int? BankBranchId { get; set; }
        public string BankAccountNumber { get; set; }
        public bool IsTaxAgency { get; set; }

        public Bank Bank { get; set; }
        public BankBranch BankBranch { get; set; }
    }
}
