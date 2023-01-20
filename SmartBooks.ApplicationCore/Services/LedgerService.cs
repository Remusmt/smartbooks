using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Helpers;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Services
{
    /// <summary>
    /// This class validates and maintains entries into the general ledgers
    /// </summary>
    public class LedgerService
    {
        private readonly TaxesService taxesService;
        private readonly JournalService journalService;
        private readonly IRepository<GeneralLedger> repository;
        private readonly LedgerAccountsService ledgerAccountsService;
        private readonly IRepository<SubLedgerBase> subLedgerRepository;
        private readonly IRepository<FinancialYear> financialYearRepository;
        private readonly IRepository<JournalDetail> journalDetailRepository;
        private readonly IRepository<DocumentSetting> documentSettingRepository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;

        private CompanyDefaults companyDefaults;
        public LedgerService(
            TaxesService taxesSer,
            JournalService journalSer,
            IRepository<GeneralLedger> repo,
            LedgerAccountsService ledgerAccountsSer,
            IRepository<SubLedgerBase> subLedgerRepo,
            IRepository<CompanyDefaults> companyDefault,
            IRepository<FinancialYear> financialYearRepo,
            IRepository<JournalDetail> journalDetailRepo,
            IRepository<DocumentSetting> documentSettingRepo)
        {
            repository = repo;
            taxesService = taxesSer;
            journalService = journalSer;
            subLedgerRepository = subLedgerRepo;
            ledgerAccountsService = ledgerAccountsSer;
            companyDefaultsRepository = companyDefault;
            financialYearRepository = financialYearRepo;
            journalDetailRepository = journalDetailRepo;
            documentSettingRepository = documentSettingRepo;
        }

        public async Task<Journal> AddJournal(
            Journal journal,
            int userId,
            int companyId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            //If journal already exists then return
            if (journal.Id > 0) return journal;

            //Journal as no entries throw error
            if (journal.JournalDetails.Count < 2)
            {
                throw new Exception("Journal must have at least two detail lines");
            }

            //get company preferences and settings and assign to private variable [companyDefaults]
            await GetCompanyDefaults(companyId);

            //if user as enabled financial year validate
            if (companyDefaults.UseFinancialYear)
            {
                await ValidateFinancialYear(companyDefaults.CurrentFinancialYear, journal.TransactionDate);
            }

            //to avoid converting JournalDetails to list everytime do it once
            List<JournalDetail> journalDetails = journal.JournalDetails.ToList();

            //Get invalid journal entries
            List<JournalDetail> journalDetailsToRemove = GetJournalEntriesWithInvalidAmounts(journalDetails);

            //Remove invalid entries
            foreach (var item in journalDetailsToRemove)
            {
                journalDetails.Remove(item);
            }

            // Validate if the entry references are valid
            //I.e Ledger account, subledgerd and taxrates
            await ValidateJournalDetailsReferences(journalDetails);

            // After removing invalid entries validate journal
            //Without rounding possibility of journal not balancing are very high
            IsJournalBalanced(journalDetails);

            //validate currencies
            if (!await ValidateCurrencies(companyDefaults.DefaultCurrency, journal.CurrencyId, journalDetails))
            {
                throw new Exception("Transactions can have only one foreign currency at a time");
            }
            //loop thorough journal details and get totals

            //Create journal from validated details, save and return to calling procedure
            decimal creditSubTotal = 0;
            decimal creditTotalTax = 0;
            decimal debitSubTotal = 0;
            decimal debitTotalTax = 0;
            //Loop entries to get the tax amount
            foreach (JournalDetail journalEntry in journalDetails)
            {
                creditSubTotal += journalEntry.Credit;
                debitSubTotal += journalEntry.Debit;
                if (debitSubTotal != 0)
                {
                    // the line represents a debit entry so the line tax amount is debit
                    debitTotalTax += journalEntry.TaxAmount;
                }
                else if (creditSubTotal != 0)
                {
                    // the line represents a credit entry so the line tax amount is credit
                    creditTotalTax += journalEntry.TaxAmount;
                }
            }
            Journal savedJournal = new Journal
            {
                CompanyId = companyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                CreditSubTotal = creditSubTotal,
                CreditTotal = creditSubTotal + creditTotalTax,
                CreditTotalTax = creditTotalTax,
                CurrencyId = journal.CurrencyId,
                DebitSubTotal = debitSubTotal,
                DebitTotal = debitSubTotal + debitTotalTax,
                DebitTotalTax = debitTotalTax,
                ExchangeRate = journal.ExchangeRate,
                Memo = journal.Memo,
                ReferenceNumber = journal.ReferenceNumber,
                TransactionDate = journal.TransactionDate,
                TransactionId = journal.TransactionId,
                TransactionStatus = TransactionStatus.Draft,
                TransactionType = journal.TransactionType,
            };

            //Populate journal details
            foreach (JournalDetail entry in journalDetails)
            {
                savedJournal.JournalDetails.Add(
                    new JournalDetail
                    {
                        BusinessUnitId = entry.BusinessUnitId,
                        CompanyId = companyId,
                        CostCenterId = entry.CostCenterId,
                        CreatedBy = userId,
                        CreatedByName = userFullName,
                        CreatedOn = dateTimeOffset,
                        Credit = entry.Credit,
                        Debit = entry.Debit,
                        LedgerAccountId = entry.LedgerAccountId,
                        Memo = entry.Memo,
                        SubLedgerId = entry.SubLedgerId,
                        Sequence = entry.Sequence,
                        TaxAmount = entry.TaxAmount,
                        TaxRateId = entry.TaxRateId,
                        TransactionId = entry.TransactionId,
                        TransactionDetailId = entry.TransactionDetailId
                    });
            }

            //Validate journal reference number
            GetJournalReference(savedJournal);

            await journalService.Add(savedJournal, userId, userFullName, dateTimeOffset, true);
            await journalService.SaveChangesAsync();
            return savedJournal;
        }

        private async void GetJournalReference(Journal journal)
        {
            if (string.IsNullOrEmpty(journal.ReferenceNumber))
            {
                bool foundRef = false;
                DocumentSetting documentSetting = await documentSettingRepository
                    .FirstOrDefaultAsync(e => e.DocumentType == "General Journal");
                while (!foundRef)
                {
                    //Get reference from helper
                    journal.ReferenceNumber = documentSetting.GetReferenceNumber();
                    //Increment settings nextNo
                    documentSetting.NextReferenceNo++;
                    //check if a similar number exists
                    if (!journalService.ReferenceNumberExists(journal.ReferenceNumber, journal.CompanyId))
                    {
                        //If non exist loop
                        foundRef = true;
                    }
                }
            }
            else
            {
                //Validate if similar reference exists
                if (journalService.ReferenceNumberExists(journal.ReferenceNumber, journal.CompanyId))
                {
                    throw new Exception("A record with a similar record already exists");
                }
            }
        }

        private async Task<bool> GetCompanyDefaults(int companyId)
        {
            companyDefaults = await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == companyId);
            //If company settings are not set throw execption, because we cann't continue
            //without these details
            if (companyDefaults == null)
            {
                throw new Exception("Incomplete system setup, consult customer support service");
            }
            return true;
        }

        private void IsJournalBalanced(List<JournalDetail> journalDetails)
        {
            //Check if journal has at least two entries after invalid entries were removed
            if (journalDetails.Count < 2)
            {
                //Throw exception if there are less than two entries
                throw new Exception("Journal must have at least two valid detail lines");
            }

            //Test if the journal is balanced
            decimal creditSubTotal = 0;
            decimal creditTotalTax = 0;
            decimal debitSubTotal = 0;
            decimal debitTotalTax = 0;
            //Loop entries to get the tax amount
            foreach (JournalDetail journalEntry in journalDetails)
            {
                creditSubTotal += journalEntry.Credit;
                debitSubTotal += journalEntry.Debit;

                //if entry tax amount is zero go to next entry
                if (journalEntry.TaxAmount == 0) continue;
                //if entry has tax amount check that there is a valid taxrate
                if (!journalEntry.TaxRateId.HasValue)
                {
                    //If taxrateid has no value throw exception
                    //NB when the system is made public for developer to use
                    //Validate if either TaxRate or TaxRateId has a value not just TaxRateId
                    //and if it is TaxRateId check if a record with the supplied id exists in the database
                    throw new Exception("Journal entry with a tax amount must have a reference to a valid tax rate");
                }

                //if valid check if it is a debit or a credit entry and assign the tax amount accordingly
                if (debitSubTotal != 0)
                {
                    // the line represents a debit entry so the line tax amount is debit
                    debitTotalTax += journalEntry.TaxAmount;
                }
                else if (creditSubTotal != 0)
                {
                    // the line represents a credit entry so the line tax amount is credit
                    creditTotalTax += journalEntry.TaxAmount;
                }
            }

            //Check if debit and credit have valid values.
            if (debitSubTotal == 0 ||
                creditSubTotal == 0)
            {
                //The values shouldn't be zero, 
                //if either debit or credit total is zero the values are invalid
                throw new Exception("Invalid journal amounts");
            }

            if (debitSubTotal + debitTotalTax !=
                creditSubTotal + creditTotalTax)
            {
                throw new Exception("Journal is not balanced");
            }

        }

        private async Task<bool> ValidateFinancialYear(int financialYearId, DateTime transactionDate)
        {
            FinancialYear financialYear = await financialYearRepository
                .FirstOrDefaultAsync(e => e.Id == financialYearId);
            // if financial year is not found throw exception
            //Financial year is not critical for the smooth running of the system
            //And this procedure can be safely skipped.
            //But it offers a way for a user who needs to ensure that transactions for within a certain
            //period a mechanism to do so. Which is a good practice
            if (financialYear == null)
            {
                throw new Exception("Incomplete system setup");
            }
            //if the user uses financial years and as closed this financial year then
            //prevent user from adding transactions, before rolling over to next financial year
            if (financialYear.Closed)
            {
                throw new Exception("Current financial year is closed");
            }
            //if user uses as enabled use of financial years and transaction date doesn't fall
            //within the current financial year throw an exception
            if (financialYear.StartDate > transactionDate
                || financialYear.EndDate < transactionDate)
            {
                throw new Exception("Invalid journal date.Transactions date should lie within the period of the financial year of the journal");
            }

            return true;
        }

        private async Task<bool> ValidateCurrencies(
            int homeCurrency,
            int journalCurrency,
            List<JournalDetail> journalDetails
            )
        {
            //Check if currencies exist. It is expensive and so pause it for now.
            //But in future when other developers will be consuming our API it will be neccessary.

            //Declare a collection to hold used currencies
            List<int> usedCurrenciesList = new List<int>
            {
                homeCurrency
            };
            if (homeCurrency != journalCurrency)
            {
                usedCurrenciesList.Add(journalCurrency);
            }
            foreach (var journalEntry in journalDetails)
            {
                //check if usedCurrenciesList.Count > 2; if true break
                //A journal can only have a maximum two currency,
                //So if there are more no need to continue checking
                if (usedCurrenciesList.Count > 2) break;

                //Fetch the account used in this entry
                //The assumption is that if the entry has an organisation it as the same currency
                LedgerAccount ledgerAccount = await ledgerAccountsService
                    .GetLedgerAccountAsync(journalEntry.LedgerAccountId);
                //if the account doesn't exist move to next entry
                //The assumption is the accounts are arleady validated b4 we get here
                //But to avoid exceptions we check. A broken connection may cause it e.t.c
                if (ledgerAccount == null) continue;

                //If currency doesn't exist in the collection add it
                if (!usedCurrenciesList.Contains(ledgerAccount.CurrencyId))
                {
                    usedCurrenciesList.Add(ledgerAccount.CurrencyId);
                }
            }
            return usedCurrenciesList.Count < 3;
        }

        private async Task<bool> ValidateJournalDetailsReferences(List<JournalDetail> journalDetails)
        {
            foreach (JournalDetail journalDetail in journalDetails)
            {
                //Check if account exists in the database
                LedgerAccount ledgerAccount = await ledgerAccountsService
                    .GetLedgerAccountAsync(journalDetail.LedgerAccountId);
                //If account doesn't exist throw exception
                if (ledgerAccount == null)
                {
                    throw new Exception("Invalid ledger account");
                }
                //If account is of type accounts receivable or payable, then the entry must
                //have a reference to a subledger
                if (ledgerAccount.AccountType == AccountType.AccountsPayable
                    || ledgerAccount.AccountType == AccountType.AccountsReceivable)
                {
                    //if OrganisationId is null then the entry is invalid 
                    if (journalDetail.SubLedgerId == null)
                    {
                        //Possible optimisation
                        if (ledgerAccount.AccountType == AccountType.AccountsPayable)
                        {
                            //A custom message for payable account type
                            throw new Exception("When you use Accounts Payable, you must choose a supplier in the Name field.");
                        }
                        else
                        {
                            //A custom message for payable account type
                            throw new Exception("When you use Accounts Receivable, you must choose a customer in the Name field.");
                        }
                    }
                    else
                    {
                        if (ledgerAccount.AccountType == AccountType.AccountsPayable)
                        {
                            SubLedgerBase subLedger = await subLedgerRepository
                                .GetByIdAsync(journalDetail.SubLedgerId.Value);

                            if (subLedger == null)
                            {
                                throw new Exception("Invalid supplier");
                            }
                            if (subLedger.CurrencyId != ledgerAccount.CurrencyId)
                            {
                                throw new Exception("The currency of your account payable and currency of the supplier must be same.");
                            }
                        }
                        if (ledgerAccount.AccountType == AccountType.AccountsReceivable)
                        {
                            SubLedgerBase subLedger = await subLedgerRepository
                               .GetByIdAsync(journalDetail.SubLedgerId.Value);
                            if (subLedger == null)
                            {
                                throw new Exception("Invalid accounts receivable references");
                            }
                            if (subLedger.CurrencyId != ledgerAccount.CurrencyId)
                            {
                                throw new Exception("The currency of your account receivable and currency of the reference must be same.");
                            }
                        }
                    }
                }
                //Validate taxrates

            }
            return true;
        }

        private List<JournalDetail> GetJournalEntriesWithInvalidAmounts(List<JournalDetail> journalDetails)
        {
            //Declare a list to hold invalid journal entries
            List<JournalDetail> journalDetailsToRemove = new List<JournalDetail>();

            //loop journal details to indentify entries with invalid values
            foreach (JournalDetail journalDetail in journalDetails)
            {
                //If entry has no amount and to invalid entries list
                if (journalDetail.Credit == 0 && journalDetail.Debit == 0)
                {
                    journalDetailsToRemove.Add(journalDetail);
                    continue;
                }
                //If the ledger account id is zero or less that an invalid value
                if (journalDetail.LedgerAccountId <= 0)
                {
                    journalDetailsToRemove.Add(journalDetail);
                    continue;
                }
            }
            return journalDetailsToRemove;
        }

        public async Task<Journal> PostJournal(
            int journalId,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            
            //Get journal by id
            Journal journal = await journalService.GetDetailedByIdAsync(journalId);

            //If journal doesn't exist throw exception 
            if (journal == null) throw new Exception("Record not found");

            await GetCompanyDefaults(journal.CompanyId);

            //Post journals that are not posted or rejected
            if (journal.TransactionStatus > TransactionStatus.Authorised)
                throw new Exception($"You can not post an '{journal.TransactionStatus}' journal");

            //Loop through all valid entries
            foreach (JournalDetail journalDetail in journal.JournalDetails)
            {
                //post journal entry
                await PostJournalLineEntry(
                    new PostJournalEntryModel
                    {
                        CompanyId = journal.CompanyId,
                        CreatedOn = dateTimeOffset,
                        Credit = journalDetail.Credit,
                        CurrencyId = journal.CurrencyId,
                        Debit = journalDetail.Debit,
                        ExchangeRate = journal.ExchangeRate,
                        JournalDetailId = journalDetail.Id,
                        JournalId = journal.Id,
                        LedgerAccountId = journalDetail.LedgerAccountId,
                        SubLedgerId = journalDetail.SubLedgerId,
                        TaxAmount = journalDetail.TaxAmount,
                        TaxRateId = journalDetail.TaxRateId,
                        TransactionDate = journal.TransactionDate,
                        UserFullName = userFullName,
                        UserId = userId
                    });
            }

            //if each entry is posted successfully update transaction and save all changes
            journal.TransactionStatus = TransactionStatus.Posted;
            await journalService.Update(journal, userId, userFullName, dateTimeOffset, true);

            await journalService.SaveChangesAsync();


            return journal;
        }

        //PostJournalEntryModel
        private async Task<bool> PostJournalLineEntry(PostJournalEntryModel postModel)
        {
            //Get ledgerAccount because it will be used by post to ledger account and post to organisation
            LedgerAccount ledgerAccount = await ledgerAccountsService
                .GetLedgerAccountAsync(postModel.LedgerAccountId);
            if (ledgerAccount == null) throw new Exception("Invalid ledger account");

            //Update ledger account balance
            UpdateLedgerAccountBalance(postModel, ledgerAccount);

            //Update supplier and customer balances
            await UpdateSubLedgerBalance(postModel);

            //Update tax amount
            if (postModel.TaxAmount > 0)
            {
                //Add tax amount
                Tax tax = await taxesService.GetTaxFromTaxRateAsync(postModel.TaxRateId.Value);
                if (tax == null) throw new Exception("Invalid tax item");
                int taxAccountId;
                if (postModel.Credit != 0)
                {
                    if (tax.SalesAccountId == null) throw new Exception("Invalid tax sales account");
                    taxAccountId = tax.SalesAccountId.Value;
                }
                else
                {
                    if (tax.PurchasesAccountId == null) throw new Exception("Invalid tax purchase account");
                    taxAccountId = tax.PurchasesAccountId.Value;
                }

                await PostJournalLineEntry(new PostJournalEntryModel
                {
                    CompanyId = postModel.CompanyId,
                    CreatedOn = postModel.CreatedOn,
                    Credit = postModel.Credit != 0 ? postModel.TaxAmount : 0,
                    CurrencyId = postModel.CurrencyId,
                    Debit = postModel.Debit != 0 ? postModel.TaxAmount : 0,
                    ExchangeRate = postModel.ExchangeRate,
                    JournalDetailId = postModel.JournalDetailId,
                    JournalId = postModel.JournalId,
                    LedgerAccountId = taxAccountId,
                    SubLedgerId = postModel.SubLedgerId,
                    TransactionDate = postModel.TransactionDate,
                    UserFullName = postModel.UserFullName,
                    UserId = postModel.UserId
                });
            }

            //Add general ledger entry
            decimal amount;
            if (postModel.Credit != 0)
            {
                amount = postModel.Credit * (-1);
            }
            else
            {
                amount = postModel.Debit;
            }
            GeneralLedger generalLedger = new GeneralLedger
            {
                Amount = GetHomeAmount(companyDefaults.DefaultCurrency, postModel.CurrencyId, amount, postModel.ExchangeRate),
                CompanyId = postModel.CompanyId,
                CreatedBy = postModel.UserId,
                CreatedByName = postModel.UserFullName,
                CreatedOn = postModel.CreatedOn,
                JournalDetailId = postModel.JournalDetailId,
                JournalId = postModel.JournalId,
                LedgerAccountId = postModel.LedgerAccountId,
                TransactionDate = postModel.TransactionDate
            };

            repository.Add(generalLedger);

            return true;
        }

        private void UpdateLedgerAccountBalance(
            PostJournalEntryModel postModel,
            LedgerAccount ledgerAccount)
        {
            //get amounts
            decimal lineAmount = postModel.Debit - postModel.Credit;
            ledgerAccount.Balance += GetHomeAmount(
                ledgerAccount.CurrencyId,
                postModel.CurrencyId,
                lineAmount,
                postModel.ExchangeRate);

            ledgerAccount.CurrencyBalance += GetEntityCurrencyAmount(
                ledgerAccount.CurrencyId,
                postModel.CurrencyId,
                lineAmount,
                postModel.ExchangeRate);

            ledgerAccountsService.SystemUpdate(ledgerAccount);
        }

        /// <summary>
        /// Update subledger balance
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        private async Task<bool> UpdateSubLedgerBalance(
            PostJournalEntryModel postModel)
        {
            if (!postModel.SubLedgerId.HasValue) return true;

            SubLedgerBase subLedger = await subLedgerRepository.GetByIdAsync(postModel.SubLedgerId.Value);

            if (subLedger == null) return true;

            decimal lineAmount = postModel.Debit - postModel.Credit;
            subLedger.Balance += GetHomeAmount(subLedger.CurrencyId, postModel.CurrencyId, lineAmount, postModel.ExchangeRate);
            subLedger.CurrencyBalance += GetEntityCurrencyAmount(subLedger.CurrencyId, postModel.CurrencyId, lineAmount, postModel.ExchangeRate);
            //Add transaction of type general ledger

            await subLedgerRepository.Update(subLedger);

            return true;
        }

        private decimal GetHomeAmount(
            int entityCurrencyId,
            int journalCurrencyId,
            decimal amount,
            decimal exchangeRate = 1)
        {
            decimal homeAmount;
            if (entityCurrencyId == journalCurrencyId)
            {
                //if entity currency is same as transaction currency
                if (entityCurrencyId == companyDefaults.DefaultCurrency)
                {
                    //Entity currency is home currency
                    homeAmount = amount;
                }
                else
                {
                    //Entity currency is foreign currency
                    //Amount is in foreign currency convert by dividing foreign amount by exchange rate
                    homeAmount = amount / exchangeRate;
                }
            }
            else
            {
                //the entity curency is not same as transaction currency
                if (journalCurrencyId == companyDefaults.DefaultCurrency)
                {
                    //Entity currency is foreign currency
                    // the amount was in home currency
                    homeAmount = amount;
                }
                else
                {
                    //Entity currency is home currency
                    //To convert from foreign currency to home currency divide
                    homeAmount = amount / exchangeRate;
                }
            }
            return homeAmount;
        }

        private decimal GetEntityCurrencyAmount(
            int entityCurrencyId,
            int journalCurrencyId,
            decimal amount,
            decimal exchangeRate = 1)
        {
            decimal foreignAmount;
            if (entityCurrencyId == journalCurrencyId)
            {
                //if entity currency is same as transaction currency
                foreignAmount = amount;
            }
            else
            {
                //the entity curency is not same as transaction currency
                if (entityCurrencyId == companyDefaults.DefaultCurrency)
                {
                    //Entity currency is foreign currency
                    //To convert from home currency to foreign currency multiply
                    foreignAmount = amount / exchangeRate;
                }
                else
                {
                    //Transaction currency is foreign currency
                    //Entity currency is home currency
                    foreignAmount = amount * exchangeRate;
                }
            }
            return foreignAmount;
        }

        public async Task<bool> DeleteJournal(int journalId)
        {
            //reverse posting to charts of accounts and organizations
            //ReverseJournal(journalId);

            //To reverse journal remove general ledger entries
            repository.DeleteRange(
                repository
                .Find(e => e.JournalId == journalId)
                .ToList());

            //Remove journal details
            journalDetailRepository.DeleteRange(
                journalDetailRepository
                .Find(e => e.JournalId == journalId)
                .ToList());

            //Remove journal
            Journal jrn = await journalService
                .GetByIdAsync(journalId);
            //await journalService.Delete(jrn);

            return true;
        }

        public async Task<Journal> ReverseJournal(int journalId)
        {
            Journal journal = await journalService.GetByIdAsync(journalId);
            if (journal == null)
            {
                throw new Exception("Journal not found");
            }
            repository.DeleteRange(
                repository
                .Find(e => e.JournalId == journalId)
                .ToList());

            List<JournalDetail> journalDetails = journalDetailRepository
                .Find(e => e.JournalId == journalId)
                .ToList();
            //get accounts and reverse amounts
            //get organisations and reverse amount posting
            //if any transaction of type general journal with this journal id remove

            journal.TransactionStatus = TransactionStatus.Draft;
            await journalService.SaveChangesAsync();

            return journal;
        }



        private class PostJournalEntryModel
        {
            public int JournalId { get; set; }
            public int JournalDetailId { get; set; }
            public int LedgerAccountId { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public decimal TaxAmount { get; set; }
            public int CurrencyId { get; set; }
            public decimal ExchangeRate { get; set; }
            public int? SubLedgerId { get; set; }
            public int? TaxRateId { get; set; }
            public int CompanyId { get; set; }
            public int UserId { get; set; }
            public string UserFullName { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public DateTime TransactionDate { get; set; }

        }

    }
}
