using SmartBooks.ApplicationCore.Helpers;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.DomainServices
{
    public class SaccoFeesService
    {
        private readonly Logger logger;
        private readonly CompanyService companyService;
        private readonly IMemberAccountTypeRepository<SaccoFee> repository;
        private readonly ILedgerAccountsRepository<LedgerAccount> ledgerRepository;
        public SaccoFeesService(
            Logger loger,
            CompanyService companySer,
            IMemberAccountTypeRepository<SaccoFee> repo,
            ILedgerAccountsRepository<LedgerAccount> ledgerRepo)
        {
            logger = loger;
            repository = repo;
            companyService = companySer;
            ledgerRepository = ledgerRepo;
        }

        public async Task<SaccoFee> GetSaccoFeeAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<SaccoFee>> GetSaccoFees(int companyId)
        {
            return await repository
                .ListAsync(new MemberAccountTypeSpecification<SaccoFee>(companyId));
        }

        public async Task<SaccoFee> Add(SaccoFee saccoFee)
        {
            if (saccoFee.CompanyId == 0)
                throw new Exception("Invalid company specified");
            CompanyDefaults companyDefaults = await companyService.GetCompanyDefaultsAsync(saccoFee.CompanyId);
            if (companyDefaults == null)
                throw new Exception("Invalid company specified");

            if (string.IsNullOrWhiteSpace(saccoFee.Description))
                throw new Exception("Description cannot be blank");

            if (repository.DescriptionExists(saccoFee.Description, saccoFee.CompanyId))
                throw new Exception("A fee with a similar description already exists");

            if (string.IsNullOrWhiteSpace(saccoFee.Code))
            {
                saccoFee.Code = repository.GetCode(saccoFee.CompanyId, companyDefaults.DefaultCodeLength);
            }
            else
            {
                if (repository.CodeExists(saccoFee.Code, saccoFee.CompanyId))
                {
                    throw new Exception("A record with a similar code already exists");
                }
            }
            
            LedgerAccount ledgerAccount = new LedgerAccount
            {
                AccountName = saccoFee.Description.GetLedgerAccountName(saccoFee.CompanyId, ledgerRepository),
                AccountType = AccountType.Income,
                CompanyId = saccoFee.CompanyId,
                CreatedBy = saccoFee.CreatedBy,
                CreatedByName = saccoFee.CreatedByName,
                CreatedOn = saccoFee.CreatedOn,
                CurrencyId = companyDefaults.DefaultCurrency,
                Description = $"System generated to track {saccoFee.Description}",
                SystemGenerated = true
            };

            saccoFee.LedgerAccount = ledgerAccount;
            repository.Add(saccoFee);
            await repository.SaveChangesAsync();

            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = saccoFee.CompanyId,
                CreatedBy = saccoFee.CreatedBy,
                CreatedByName = saccoFee.CreatedByName,
                CreatedOn = saccoFee.CreatedOn,
                RecordId = saccoFee.Id,
                RecordType = RecordType.SaccoFee,
                SerializedRecord = logger.SeliarizeObject(saccoFee)
            });
            return saccoFee;
        }

        public async Task<SaccoFee> Update(
            SaccoFee saccoFee,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(saccoFee.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(saccoFee.Id, saccoFee.Description, saccoFee.CompanyId))
            {
                throw new Exception($"Updating description with {saccoFee.Description} would create a duplicate");
            }
            if (!string.IsNullOrWhiteSpace(saccoFee.Code))
            {
                if (repository.DuplicateCode(saccoFee.Id, saccoFee.Code, saccoFee.CompanyId))
                {
                    throw new Exception($"Updating code with {saccoFee.Code} would create a duplicate");
                }
            }

            LedgerAccount ledgerAccount = await ledgerRepository.GetByIdAsync(saccoFee.LedgerAccountId);
            if (ledgerAccount.AccountName.GetCleanLedgerAccountName() != saccoFee.Description)
            {
                ledgerAccount.AccountName = saccoFee.Description
                    .GetLedgerAccountName(saccoFee.CompanyId, ledgerRepository);
                await ledgerRepository.Update(ledgerAccount);
            }
            await repository.Update(saccoFee);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = saccoFee.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = saccoFee.Id,
                RecordType = RecordType.SaccoFee,
                SerializedRecord = logger.SeliarizeObject(saccoFee)
            });
            return saccoFee;
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            SaccoFee sacooFee = await repository.GetByIdAsync(Id);
            if (sacooFee == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(sacooFee);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = sacooFee.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = sacooFee.Id,
                RecordType = RecordType.SaccoFee,
                SerializedRecord = logger.SeliarizeObject(sacooFee)
            });

            return Id;
        }

    }
}
