using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.SaccoServices;
using SmartBooks.ApplicationCore.SchoolServices;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Domains.SchoolEntities;
using SmartBooks.Persitence.Data.Context;
using SmartBooks.Persitence.Data.Repositories;
using System.Text;

namespace SmartBooks.Api.Helpers
{
    public static class StartupServiceHelpers
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Bank>, Repository<Bank>>();
            services.AddScoped<IBinRepository<Bin>, BinRepository<Bin>>();
            services.AddScoped<ITaxRepository<Tax>, TaxRepository<Tax>>();
            services.AddScoped<IRepository<TaxRate>, Repository<TaxRate>>();
            services.AddScoped<IRepository<BinCard>, Repository<BinCard>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<Country>, Repository<Country>>();
            services.AddScoped<IRepository<Journal>, Repository<Journal>>();
            services.AddScoped<IRepository<Currency>, Repository<Currency>>();
            services.AddScoped<IRepository<AuditLog>, Repository<AuditLog>>();
            services.AddScoped<IRepository<LedgerEntry>, Repository<LedgerEntry>>();
            services.AddScoped<IRepository<FinancialYear>, Repository<FinancialYear>>();
            services.AddScoped<IRepository<GeneralLedger>, Repository<GeneralLedger>>();
            services.AddScoped<IRepository<JournalDetail>, Repository<JournalDetail>>();
            services.AddScoped<IRepository<UomConversion>, Repository<UomConversion>>();
            services.AddScoped<IRepository<SubLedgerBase>, Repository<SubLedgerBase>>();
            services.AddScoped<IRepository<InventoryLedger>, Repository<InventoryLedger>>();
            services.AddScoped<IRepository<CompanyDefaults>, Repository<CompanyDefaults>>();
            services.AddScoped<IRepository<DocumentSetting>, Repository<DocumentSetting>>();
            services.AddScoped<IJournalRepository<Journal>, JournalRepository<Journal>>();
            services.AddScoped<IWarehouseRepository<Warehouse>, WarehouseRepository<Warehouse>>();
            services.AddScoped<ICategoryRepository<CostCenter>, CategoryRepository<CostCenter>>();
            services.AddScoped<ICategoryRepository<CustomerType>, CategoryRepository<CustomerType>>();
            services.AddScoped<ICategoryRepository<SupplierType>, CategoryRepository<SupplierType>>();
            services.AddScoped<IOrganisationRepository<Customer>, OrganisationRepository<Customer>>();
            services.AddScoped<IOrganisationRepository<Supplier>, OrganisationRepository<Supplier>>();
            services.AddScoped<IPaymentTermRepository<PaymentTerm>, PaymentTermRepository<PaymentTerm>>();
            services.AddScoped<ICategoryRepository<InventoryCategory>, CategoryRepository<InventoryCategory>>();
            services.AddScoped<IUnitofMeasureRepository<UnitofMeasure>, UnitofMeasureRepository<UnitofMeasure>>();
            services.AddScoped<IInventoryItemRepository<InventoryItem>, InventoryItemRepository<InventoryItem>>();
            services.AddScoped<ILedgerAccountsRepository<LedgerAccount>, LedgerAccountsRepository<LedgerAccount>>();

            // Sacco

            services.AddScoped<IRepository<Attachment>, Repository<Attachment>>();
            services.AddScoped<IRepository<Address>, Repository<Address>>();
            services.AddScoped<IRepository<MemberAttachment>, Repository<MemberAttachment>>();
            services.AddScoped<IRepository<NextOfKin>, Repository<NextOfKin>>();
            services.AddScoped<IRepository<MemberApproval>, Repository<MemberApproval>>();
            services.AddScoped<IRepository<SaccoSettings>, Repository<SaccoSettings>>();
            services.AddScoped<IMemberRepository<Member>, MemberRepository<Member>>();

            // School
            services.AddScoped<ISchoolBaseRepository<Block>, SchoolBaseRepository<Block>>();
            services.AddScoped<ISchoolBaseRepository<ClassRoom>, SchoolBaseRepository<ClassRoom>>();
            services.AddScoped<ISchoolBaseRepository<Dormitory>, SchoolBaseRepository<Dormitory>>();
            services.AddScoped<ISchoolBaseRepository<Level>, SchoolBaseRepository<Level>>();
            services.AddScoped<ISchoolBaseRepository<Subject>, SchoolBaseRepository<Subject>>();
            services.AddScoped<ISchoolBaseRepository<TeachingDepartment>, SchoolBaseRepository<TeachingDepartment>>();
            services.AddScoped<ISchoolBaseRepository<UtilityRoom>, SchoolBaseRepository<UtilityRoom>>();
            // Domain Services
            services.AddScoped(typeof(Logger));
            services.AddScoped(typeof(TaxesService));
            services.AddScoped(typeof(CompanyService));
            services.AddScoped(typeof(JournalService));
            services.AddScoped(typeof(CustomerService));
            services.AddScoped(typeof(SupplierService));
            services.AddScoped(typeof(WarehouseService));
            services.AddScoped(typeof(PaymentTermService));
            services.AddScoped(typeof(InventoryItemService));
            services.AddScoped(typeof(LedgerAccountsService));
            services.AddScoped(typeof(UnitsofMeasureService));
            services.AddScoped(typeof(CategoriesService<CostCenter>));
            services.AddScoped(typeof(OrganisationsServices<Customer>));
            services.AddScoped(typeof(OrganisationsServices<Supplier>));
            services.AddScoped(typeof(CategoriesService<CustomerType>));
            services.AddScoped(typeof(CategoriesService<SupplierType>));
            services.AddScoped(typeof(CategoriesService<InventoryCategory>));

            //Sacco Services
            services.AddScoped(typeof(MemberService));

            //School Services
            services.AddScoped(typeof(BlockService));
            services.AddScoped(typeof(ClassRoomService));
            services.AddScoped(typeof(DormitoryService));
            services.AddScoped(typeof(LevelService));
            services.AddScoped(typeof(SubjectService));
            services.AddScoped(typeof(TeachingDepartmentService));
            services.AddScoped(typeof(UtilityRoomService));

            //Module services
            services.AddScoped(typeof(LedgerService));
            services.AddScoped(typeof(InventoryService));

            //Adding a services to generate html
            services.AddScoped(typeof(HtmlHelper));

            //Adding DinkToPdf
            services.AddSingleton<ITools, PdfTools>();
            services.AddSingleton(typeof(SynchronizedConverter));
        }

        public static void ConfigureAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Use objects that derive from identity TKEY
            services.AddDefaultIdentity<ApplicationUser>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddEntityFrameworkStores<SmartBooksContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "witsoft.co.ke",
                        ValidAudience = "witsoft.co.ke",
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }

        public static void ConfigureContext(
            this IServiceCollection services, 
            AppSettings appSettings,
            IConfiguration Configuration)
        {
            string MysqlConnection = Configuration.GetConnectionString("MysqlConnection");
            
            switch (appSettings.DbmsType)
            {
                case DbmsType.SqlServer:
                    services.AddDbContext<SmartBooksContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
                    break;
                case DbmsType.Mysql:
                    services.AddDbContext<SmartBooksContext>(options =>
                        options.UseMySql(MysqlConnection, ServerVersion.AutoDetect(MysqlConnection)));
                    break;
            }
        }

    }
}
