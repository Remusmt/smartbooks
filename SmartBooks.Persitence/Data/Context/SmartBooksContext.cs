using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Domains.SaccoEntities.Transactions;
using SmartBooks.Domains.SchoolEntities;
using SmartBooks.Persitence.Data.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SmartBooks.Persitence.Data.Context
{
    public class SmartBooksContextFactory : IDesignTimeDbContextFactory<SmartBooksContext>
    {
        //public SmartBooksContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<SmartBooksContext>();
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-LPOGMEO\\SQLEXPRESS01;Database=SmartBooksContextDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    return new SmartBooksContext(optionsBuilder.Options);
        //}

        public SmartBooksContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmartBooksContext>();
            string strConn = "server=localhost;database=smartbookscontextdb;user=root;password=N33m@1118;port=3307";
            optionsBuilder.UseMySql(strConn, ServerVersion.AutoDetect(strConn));
            return new SmartBooksContext(optionsBuilder.Options);
        }
    }
    public class SmartBooksContext : DbContext
    {
        public SmartBooksContext(DbContextOptions<SmartBooksContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UserRole>().HasKey(e => e.RoleId);
            builder.Entity<SimpleValue>(e => e.HasNoKey());
            //builder.

            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(6);
            }
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankBranch> BankBranches { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<CompanyDefaults> CompanyDefaults { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }

        public DbSet<CurrencyConversion> CurrencyConversions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerProject> CustomerProjects { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<SubLedgerBase> SubLedgerBases { get; set; }
        public DbSet<OrganisationAddress> OrganisationAddresses { get; set; }


        public DbSet<LedgerAccount> LedgerAccounts { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<GeneralLedger> GeneralLedgers { get; set; }
        public DbSet<LedgerEntry> LedgerEntries { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }


        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<BinCard> BinCards { get; set; }
        public DbSet<UnitofMeasure> UnitofMeasures { get; set; }
        public DbSet<UomConversion> UomConversions { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryLedger> InventoryLedgers { get; set; }
        public DbSet<ReorderLevel> ReorderLevels { get; set; }
        
        public DbSet<DocumentSetting> DocumentSettings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }

        //School Entities
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeachingDepartment> TeachingDepartments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<SchoolTerm> SchoolTerms { get; set; }
        public DbSet<Dormitory> Dormitories { get; set; }
        public DbSet<UtilityRoom> UtilityRooms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassRegister> ClassRegisters { get; set; }
        public DbSet<ClassRegisterDetail> ClassRegisterDetails { get; set; }

        //Sacco entities
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberApproval> MemberApprovals { get; set; }
        public DbSet<MemberAttachment> MemberAttachments { get; set; }
        public DbSet<NextOfKin> NextOfKins { get; set; }
        public DbSet<MemberAccountType> MemberAccountTypes { get; set; }
        public DbSet<SaccoSettings> SaccoSettings { get; set; }
        public DbSet<SaccoFee> SaccoFees { get; set; }
        public DbSet<SavingsItem> SavingsItems { get; set; }
        public DbSet<SharesItem> SharesItems { get; set; }
        public DbSet<LoanProduct> LoanProducts { get; set; }
        public DbSet<MemberAccount> MemberAccounts { get; set; }

        //Source Documents
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<AddFees> AddFees { get; set; }
        public DbSet<FeeItem> FeeItems { get; set; }

        //string test
        [NotMapped]
        public DbSet<SimpleValue> SingleValue { get; set; }

    }
}
