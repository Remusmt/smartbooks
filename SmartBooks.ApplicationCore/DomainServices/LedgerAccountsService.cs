using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.DomainServices
{
    public class LedgerAccountsService
    {
        private readonly Logger logger;
        private readonly ILedgerAccountsRepository<LedgerAccount> repository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;

        public LedgerAccountsService(
            Logger loger,
            IRepository<CompanyDefaults> companyDefaultsRepo,
            ILedgerAccountsRepository<LedgerAccount> accountsRepository)
        {
            logger = loger;
            repository = accountsRepository;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<LedgerAccount> GetLedgerAccountAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<LedgerAccount>> GetLedgerAccounts(int companyId)
        {
            List<LedgerAccount> ledgerAccounts = await repository
                .ListAsync(new LedgerAccountSpecification(companyId));
            List<LedgerAccount> returnList = new List<LedgerAccount>();
            foreach (var account in ledgerAccounts)
            {
                if (!account.ParentAccountId.HasValue)
                {
                    account.TotalBalance += account.Balance;
                    account.TotalCurrencyBalance += account.CurrencyBalance;
                    returnList.Add(account);
                }
                else
                {
                    if (account.ParentAccount != null)
                    {
                        account.TotalBalance += account.Balance;
                        account.TotalCurrencyBalance += account.CurrencyBalance;
                        account.ParentAccount.TotalBalance += account.TotalBalance;
                        account.ParentAccount.TotalCurrencyBalance += account.TotalCurrencyBalance;
                    }
                }
            }
            return returnList
                .OrderBy(e => e.AccountType)
                .ToList();
        }

        public async Task<List<LedgerAccount>> GetPostingLedgerAccounts(int companyId)
        {
            CompanyDefaults companyDefaults = await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == companyId);
            List<LedgerAccount> ledgerAccounts = await repository
                 .ListAsync(new LedgerAccountSpecification(companyId));
            if (companyDefaults.AllowPostingToParentAccount)
            {
                return ledgerAccounts
                    .Select(account =>
                             new LedgerAccount
                             {
                                 Id = account.Id,
                                 AccountName = account.AccountName,
                                 AccountType = account.AccountType,
                                 AccountNumber = account.AccountNumber,
                                 Balance = account.Balance,
                                 BankAccountNo = account.BankAccountNo,
                                 DetailAccountType = account.DetailAccountType,
                                 CompanyId = account.CompanyId,
                                 CreatedBy = account.CreatedBy,
                                 CurrencyBalance = account.CurrencyBalance,
                                 CurrencyId = account.CurrencyId,
                                 Description = account.Description,
                                 HasOverDraft = account.HasOverDraft,
                                 Height = account.Height,
                                 OverDraftLimit = account.OverDraftLimit,
                                 ParentAccountId = account.ParentAccountId,
                                 TaxRateId = account.TaxRateId,
                                 TotalBalance = account.TotalBalance,
                                 AddToDashboard = account.AddToDashboard,
                                 ShowInPettyCash = account.ShowInPettyCash,
                                 IsDeleted = account.IsDeleted,
                                 UpdateCode = account.UpdateCode
                             })
                    .OrderBy(e => e.AccountType)
                    .ToList();
            }
            else
            {
                return ledgerAccounts
               .Where(e => e.ChildAccounts.Count == 0)
               .Select(account =>
                        new LedgerAccount
                        {
                            Id = account.Id,
                            AccountName = account.AccountName,
                            AccountType = account.AccountType,
                            AccountNumber = account.AccountNumber,
                            Balance = account.Balance,
                            BankAccountNo = account.BankAccountNo,
                            DetailAccountType = account.DetailAccountType,
                            CompanyId = account.CompanyId,
                            CreatedBy = account.CreatedBy,
                            CurrencyBalance = account.CurrencyBalance,
                            CurrencyId = account.CurrencyId,
                            Description = account.Description,
                            HasOverDraft = account.HasOverDraft,
                            Height = account.Height,
                            OverDraftLimit = account.OverDraftLimit,
                            ParentAccountId = account.ParentAccountId,
                            TaxRateId = account.TaxRateId,
                            TotalBalance = account.TotalBalance,
                            AddToDashboard = account.AddToDashboard,
                            ShowInPettyCash = account.ShowInPettyCash,
                            IsDeleted = account.IsDeleted,
                            UpdateCode = account.UpdateCode

                        })
               .OrderBy(e => e.AccountType)
               .ToList();
            }


        }

        public async Task<LedgerAccount> Add(LedgerAccount ledgerAccount)
        {
            if (ledgerAccount.CompanyId == 0)
                throw new Exception("An error occured while saving account");

            if (string.IsNullOrWhiteSpace(ledgerAccount.AccountName))
                throw new Exception("Account name cannot be blank");

            if (repository.AccountNameExists(ledgerAccount.AccountName, ledgerAccount.CompanyId))
                throw new Exception("An account with a similar account name already exists");

            CompanyDefaults companyDefaults = await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == ledgerAccount.CompanyId);
            if (companyDefaults.UseAccountNumbers)
            {
                if (string.IsNullOrWhiteSpace(ledgerAccount.AccountNumber))
                    throw new Exception("Account number cannot be blank");

                if (repository.AccountNumberExists(ledgerAccount.AccountNumber, ledgerAccount.CompanyId))
                    throw new Exception("An account with a similar account number already exists");
            }
            LedgerAccount parentAccount = null;
            if (ledgerAccount.ParentAccountId.HasValue)
            {
                parentAccount = await repository
                        .GetByIdAsync(ledgerAccount.ParentAccountId.Value);
            }
            else
            {
                ledgerAccount.Height = 0;
            }
            if (parentAccount != null)
            {
                if (parentAccount.AccountType != ledgerAccount.AccountType)
                {
                    throw new Exception("Parent and sub account must be of the same account type");
                }
                ledgerAccount.Height = (byte)(parentAccount.Height + 1);
            }

            if (ledgerAccount.Height > 5)
            {
                throw new Exception("Invalid parent, it will create to many levels of accounts");
            }

            if (ledgerAccount.CurrencyId == 0)
            {
                // parent currency and child currency must match
                if (ledgerAccount.ParentAccountId.HasValue)
                {
                    if (parentAccount != null)
                    {
                        ledgerAccount.CurrencyId = parentAccount.CurrencyId;
                    }
                    else
                    {
                        ledgerAccount.CurrencyId = companyDefaults.DefaultCurrency;
                    }

                }
                else
                {
                    ledgerAccount.CurrencyId = companyDefaults.DefaultCurrency;
                }
            }
            else
            {
                // Accounts other than Payable, receivable and cash should have company default currency 
                if (ledgerAccount.AccountType != AccountType.AccountsPayable
                    || ledgerAccount.AccountType != AccountType.AccountsReceivable
                    || ledgerAccount.AccountType != AccountType.Cash)
                {
                    ledgerAccount.CurrencyId = companyDefaults.DefaultCurrency;
                }

                // parent currency and child currency must match
                if (parentAccount != null)
                {
                    if (parentAccount.CurrencyId != ledgerAccount.CurrencyId)
                    {
                        throw new Exception("Account should have same currency as parent account");
                    }
                }

            }

            repository.Add(ledgerAccount);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = ledgerAccount.CompanyId,
                CreatedBy = ledgerAccount.CreatedBy,
                CreatedByName = ledgerAccount.CreatedByName,
                CreatedOn = ledgerAccount.CreatedOn,
                RecordId = ledgerAccount.Id,
                RecordType = RecordType.LedgerAccount,
                SerializedRecord = logger.SeliarizeObject(ledgerAccount)
            });
            return ledgerAccount;
        }

        public async Task<LedgerAccount> Update(
            LedgerAccount ledgerAccount,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset,
            bool systemGenerated = false)
        {
            if (string.IsNullOrWhiteSpace(ledgerAccount.AccountName))
            {
                throw new Exception("Account name cannot be blank");
            }
            if (repository.DuplicateAccountName(ledgerAccount.Id, ledgerAccount.AccountName, ledgerAccount.CompanyId))
            {
                throw new Exception($"Updating account name with {ledgerAccount.AccountName} would create a duplicate");
            }

            CompanyDefaults companyDefaults = await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == ledgerAccount.CompanyId);

            if (companyDefaults.UseAccountNumbers)
            {
                if (string.IsNullOrWhiteSpace(ledgerAccount.AccountNumber))
                    throw new Exception("Account number cannot be blank");

                if (repository.DuplicateAccountNumber(ledgerAccount.Id, ledgerAccount.AccountNumber, ledgerAccount.CompanyId))
                    throw new Exception($"Updating account number with {ledgerAccount.AccountNumber} would create a duplicate record");
            }

            LedgerAccount parentAccount = null;
            if (ledgerAccount.ParentAccountId.HasValue)
            {
                parentAccount = await repository
                        .GetByIdAsync(ledgerAccount.ParentAccountId.Value);
            }
            else
            {
                ledgerAccount.Height = 0;
            }
            if (parentAccount != null)
            {
                if (parentAccount.AccountType != ledgerAccount.AccountType)
                {
                    throw new Exception("Parent and sub account must be of the same account type");
                }
                ledgerAccount.Height = (byte)(parentAccount.Height + 1);
            }

            if (ledgerAccount.CurrencyId == 0)
            {
                if (parentAccount != null)
                {
                    ledgerAccount.CurrencyId = parentAccount.CurrencyId;
                }
                else
                {
                    ledgerAccount.CurrencyId = companyDefaults.DefaultCurrency;
                }
            }
            else
            {
                // Accounts other than Payable, receivable and cash should have company default currency 
                if (ledgerAccount.AccountType != AccountType.AccountsPayable
                    && ledgerAccount.AccountType != AccountType.AccountsReceivable
                    && ledgerAccount.AccountType != AccountType.Cash)
                {
                    ledgerAccount.CurrencyId = companyDefaults.DefaultCurrency;
                }

                // parent currency and child currency must match
                if (parentAccount != null)
                {
                    if (parentAccount.CurrencyId != ledgerAccount.CurrencyId)
                    {
                        throw new Exception("Account should have same currency as parent account");
                    }
                }
            }

            await repository.Update(ledgerAccount);
            if (!systemGenerated)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = ledgerAccount.CompanyId,
                    CreatedBy = userId,
                    CreatedByName = userFullName,
                    CreatedOn = dateTimeOffset,
                    RecordId = ledgerAccount.Id,
                    RecordType = RecordType.LedgerAccount,
                    SerializedRecord = logger.SeliarizeObject(ledgerAccount)
                });
            }

            return ledgerAccount;
        }

        public void SystemUpdate(LedgerAccount ledgerAccount)
        {
            repository.Update(ledgerAccount);
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            LedgerAccount ledgerAccount = await repository.GetByIdAsync(Id);
            if (ledgerAccount == null)
            {
                throw new Exception("Record not found");
            }
            //repository.RemoveParent(ledgerAccount.Id);
            repository.SoftDelete(ledgerAccount);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = ledgerAccount.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = ledgerAccount.Id,
                RecordType = RecordType.LedgerAccount,
                SerializedRecord = logger.SeliarizeObject(ledgerAccount)
            });

            return Id;
        }
    }
}
