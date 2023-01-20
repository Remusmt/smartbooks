using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.SaccoServices;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Persitence.Data.Context;
using SmartBooks.Persitence.Data.Repositories;
using System.IO;

namespace SmartBooks.UnitTests.DependencyManagement
{
    public class DependancyInjector
    {
        private readonly IConfigurationRoot configuration;
        private readonly SmartBooksContext context;

        //Dependancies
        public IRepository<Bank> BankRepository
        {
            get
            {
                return new Repository<Bank>(context);
            }
        }

        public IBinRepository<Bin> BinRepository
        {
            get
            {
                return new BinRepository<Bin>(context);
            }
        }

        public ITaxRepository<Tax> TaxRepository
        {
            get
            {
                return new TaxRepository<Tax>(context);
            }
        }
        public IRepository<TaxRate> TaxRateRepository
        {
            get
            {
                return new Repository<TaxRate>(context);
            }
        }
        public IRepository<BinCard> BinCardRepository
        {
            get
            {
                return new Repository<BinCard>(context);
            }
        }
        public IRepository<Company> CompanyRepository
        {
            get
            {
                return new Repository<Company>(context);
            }
        }
        public IRepository<Country> CountryRepository
        {
            get
            {
                return new Repository<Country>(context);
            }
        }
        public IRepository<Currency> CurrencyRepository
        {
            get
            {
                return new Repository<Currency>(context);
            }
        }
        public IRepository<AuditLog> AuditLogRepository
        {
            get
            {
                return new Repository<AuditLog>(context);
            }
        }
        public IRepository<LedgerEntry> LedgerEntryRepository
        {
            get
            {
                return new Repository<LedgerEntry>(context);
            }
        }
        public IRepository<FinancialYear> FinancialYearRepository
        {
            get
            {
                return new Repository<FinancialYear>(context);
            }
        }
        public IRepository<GeneralLedger> GeneralLedgerRepository
        {
            get
            {
                return new Repository<GeneralLedger>(context);
            }
        }
        public IRepository<JournalDetail> JournalDetailRepository
        {
            get
            {
                return new Repository<JournalDetail>(context);
            }
        }
        public IRepository<UomConversion> UomConversionRepository
        {
            get
            {
                return new Repository<UomConversion>(context);
            }
        }
        public IRepository<InventoryLedger> InventoryLedgerRepository
        {
            get
            {
                return new Repository<InventoryLedger>(context);
            }
        }
        public IRepository<CompanyDefaults> CompanyDefaultsRepository
        {
            get
            {
                return new Repository<CompanyDefaults>(context);
            }
        }
        public IRepository<DocumentSetting> DocumentSettingRepository
        {
            get
            {
                return new Repository<DocumentSetting>(context);
            }
        }
        public IRepository<Attachment> AttachmentRepository
        {
            get
            {
                return new Repository<Attachment>(context);
            }
        }
        public IRepository<Address> AddressRepository
        {
            get
            {
                return new Repository<Address>(context);
            }
        }
        public IRepository<MemberAttachment> MemberAttachmentRepository
        {
            get
            {
                return new Repository<MemberAttachment>(context);
            }
        }
        public IRepository<NextOfKin> NextOfKinRepository
        {
            get
            {
                return new Repository<NextOfKin>(context);
            }
        }
        public IRepository<MemberApproval> MemberApprovalRepository
        {
            get
            {
                return new Repository<MemberApproval>(context);
            }
        }

        public IRepository<SaccoSettings> SaccoSettingsRepository
        {
            get
            {
                return new Repository<SaccoSettings>(context);
            }
        }
        public IMemberRepository<Member> MemberRepository
        {
            get
            {
                return new MemberRepository<Member>(context);
            }
        }
        public IJournalRepository<Journal> JournalRepository
        {
            get
            {
                return new JournalRepository<Journal>(context);
            }
        }
        public IWarehouseRepository<Warehouse> WarehouseRepository
        {
            get
            {
                return new WarehouseRepository<Warehouse>(context);
            }
        }
        public ICategoryRepository<CostCenter> CostCenterRepository
        {
            get
            {
                return new CategoryRepository<CostCenter>(context);
            }
        }
        public ICategoryRepository<CustomerType> CustomerTypeRepository
        {
            get
            {
                return new CategoryRepository<CustomerType>(context);
            }
        }
        public ICategoryRepository<SupplierType> SupplierTypeRepository
        {
            get
            {
                return new CategoryRepository<SupplierType>(context);
            }
        }
        public ICategoryRepository<InventoryCategory> InventoryCategoryRepository
        {
            get
            {
                return new CategoryRepository<InventoryCategory>(context);
            }
        }
        public IOrganisationRepository<Customer> CustomerRepository
        {
            get
            {
                return new OrganisationRepository<Customer>(context);
            }
        }
        public IOrganisationRepository<Supplier> SupplierRepository
        {
            get
            {
                return new OrganisationRepository<Supplier>(context);
            }
        }
        public IPaymentTermRepository<PaymentTerm> PaymentTermRepository
        {
            get
            {
                return new PaymentTermRepository<PaymentTerm>(context);
            }
        }
        public IUnitofMeasureRepository<UnitofMeasure> UnitofMeasureRepository
        {
            get
            {
                return new UnitofMeasureRepository<UnitofMeasure>(context);
            }
        }
        public IInventoryItemRepository<InventoryItem> InventoryItemRepository
        {
            get
            {
                return new InventoryItemRepository<InventoryItem>(context);
            }
        }
        public ILedgerAccountsRepository<LedgerAccount> LedgerAccountsRepository
        {
            get
            {
                return new LedgerAccountsRepository<LedgerAccount>(context);
            }
        }
        public IRepository<SubLedgerBase> SubLedgerRepository
        {
            get
            {
                return new Repository<SubLedgerBase>(context);
            }
        }
        public IMemberAccountTypeRepository<SaccoFee> SaccoFeesRepository
        {
            get
            {
                return new MemberAccountTypeRepository<SaccoFee>(context);
            }
        }

        public IMemberAccountRepository<MemberAccount> MemberAccountRepository
        {
            get
            {
                return new MemberAccountRepository<MemberAccount>(context);
            }
        }

        public Logger Logger
        {
            get
            {
                return new Logger(AuditLogRepository);
            }
        }
        public TaxesService TaxesService
        {
            get
            {
                return new TaxesService(Logger, TaxRateRepository, TaxRepository);
            }
        }
        public CompanyService CompanyService
        {
            get
            {
                return new CompanyService(
                    BankRepository,
                    CompanyRepository,
                    CountryRepository,
                    CurrencyRepository,
                    SaccoSettingsRepository,
                    CompanyDefaultsRepository);
            }
        }
        public CustomerService CustomerService
        {
            get
            {
                return new CustomerService(Logger, CustomerRepository);
            }
        }
        public SupplierService SupplierService
        {
            get
            {
                return new SupplierService(Logger, CompanyDefaultsRepository, SupplierRepository);
            }
        }
        public WarehouseService WarehouseService
        {
            get
            {
                return new WarehouseService(Logger, WarehouseRepository, BinRepository);
            }
        }
        public PaymentTermService PaymentTermService
        {
            get
            {
                return new PaymentTermService(Logger, PaymentTermRepository);
            }
        }
        public InventoryItemService InventoryItemService
        {
            get
            {
                return new InventoryItemService(Logger, InventoryItemRepository);
            }
        }
        public LedgerAccountsService LedgerAccountsService
        {
            get
            {
                return new LedgerAccountsService(
                    Logger,
                    CompanyDefaultsRepository,
                    LedgerAccountsRepository);
            }
        }
        public UnitsofMeasureService UnitsofMeasureService
        {
            get
            {
                return new UnitsofMeasureService(
                    Logger,
                    UomConversionRepository,
                    UnitofMeasureRepository);
            }
        }
        public CategoriesService<CostCenter> CostcenterService
        {
            get
            {
                return new CategoriesService<CostCenter>(Logger, CostCenterRepository);
            }
        }
        public CategoriesService<CustomerType> CustomerTypeService
        {
            get
            {
                return new CategoriesService<CustomerType>(Logger, CustomerTypeRepository);
            }
        }
        public CategoriesService<SupplierType> SupplierTypeService
        {
            get
            {
                return new CategoriesService<SupplierType>(Logger, SupplierTypeRepository);
            }
        }
        public CategoriesService<InventoryCategory> InventoryCategoryService
        {
            get
            {
                return new CategoriesService<InventoryCategory>(Logger, InventoryCategoryRepository);
            }
        }
        public OrganisationsServices<Customer> CustomerOrganisationService
        {
            get
            {
                return new OrganisationsServices<Customer>(
                    Logger,
                    CompanyDefaultsRepository,
                    CustomerRepository);
            }
        }
        public OrganisationsServices<Supplier> SupplierOrganisationService
        {
            get
            {
                return new OrganisationsServices<Supplier>(
                    Logger,
                    CompanyDefaultsRepository,
                    SupplierRepository);
            }
        }
        public JournalService JournalService
        {
            get
            {
                return new JournalService(Logger, JournalRepository);
            }
        }
        public LedgerService LedgerService
        {
            get
            {
                return new LedgerService(
                    TaxesService,
                    JournalService,
                    GeneralLedgerRepository,
                    LedgerAccountsService,
                    SubLedgerRepository,
                    CompanyDefaultsRepository,
                    FinancialYearRepository,
                    JournalDetailRepository,
                    DocumentSettingRepository);
            }
        }
        public InventoryService InventoryService
        {
            get
            {
                return new InventoryService(
                    BinCardRepository,
                    UomConversionRepository,
                    InventoryLedgerRepository,
                    InventoryItemService);
            }
        }

        public MemberService MemberService
        {
            get
            {
                return new MemberService(
                    Logger,
                    CompanyService,
                    AddressRepository,
                    MemberRepository,
                    NextOfKinRepository,
                    AttachmentRepository,
                    MemberAttachmentRepository,
                    MemberApprovalRepository);
            }
        }

        public SaccoFeesService SaccoFeesService
        {
            get
            {
                return new SaccoFeesService(
                    Logger,
                    CompanyService,
                    SaccoFeesRepository,
                    LedgerAccountsRepository);
            }
        }

        public MemberAccountsService MemberAccountsService
        {
            get
            {
                return new MemberAccountsService(
                    Logger,
                    MemberService,
                    SaccoFeesService,
                    MemberAccountRepository);
            }
        }


        public int CompanyId
        {
            get
            {
                return 4;
            }
        }
        public int UserId
        {
            get
            {
                return 1;
            }
        }

        public string UserFullName
        {
            get
            {
                return "School Admin";
            }
        }
        public DependancyInjector()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("MysqlConnection");
            DbContextOptions<SmartBooksContext> options = new DbContextOptionsBuilder<SmartBooksContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
            context = new SmartBooksContext(options);
        }

    }
}
