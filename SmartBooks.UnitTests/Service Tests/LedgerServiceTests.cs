using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartBooks.Domains.Entities;
using SmartBooks.UnitTests.DependencyManagement;
using System;
using System.Threading.Tasks;

namespace SmartBooks.UnitTests.Service_Tests
{
    [TestClass]
    public class LedgerServiceTests
    {
        private readonly DependancyInjector dependancyInjector;
        public LedgerServiceTests()
        {
            dependancyInjector = new DependancyInjector();
        }

        [TestMethod]
        public async Task AddSimpleJournalTest()
        {
            //Create a journal
            Journal journal = new Journal
            {
                CurrencyId = 118,
                ExchangeRate = 1,
                Memo = "Simple journal entry",
                ReferenceNumber = "1",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 1,
                    Sequence = 1
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5050,
                    LedgerAccountId = 2,
                    Sequence = 2
                });
            Journal savedJournal = await dependancyInjector.LedgerService.AddJournal(
                journal, 
                dependancyInjector.UserId, 
                dependancyInjector.CompanyId, 
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(savedJournal.Id > 0);
        }

        [TestMethod]
        public async Task AddJournalWithOrganisations()
        {
            //Create a journal
            //If ledger account type is not of type payable or receivable don't validate organisation
            Journal journal = new Journal
            {
                CurrencyId = 118,
                ExchangeRate = 1,
                Memo = "Simple journal with organisation reference",
                ReferenceNumber = "1",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 1,
                    Sequence = 1,
                    SubLedgerId = 10
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5050,
                    LedgerAccountId = 2,
                    Sequence = 2
                });
            Journal savedJournal = await dependancyInjector.LedgerService.AddJournal(
                journal,
                dependancyInjector.UserId,
                dependancyInjector.CompanyId,
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(savedJournal.Id > 0);
        }

        [TestMethod]
        public void AddAccountsPayableJournalWithoutSupplier()
        {
            //Create a journal
            Journal journal = new Journal
            {
                CurrencyId = 118,
                ExchangeRate = 1,
                Memo = "Accounts Payable journal entry",
                ReferenceNumber = "1",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 5,
                    Sequence = 1
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5050,
                    LedgerAccountId = 2,
                    Sequence = 2
                });

            //Journal entry with ledger account of type accounts payable, without a reference
            //should throw an error with a message :-
            //"When you use Accounts Payable, you must choose a supplier in the Name field."
            Assert.ThrowsExceptionAsync<Exception>(
                async () => await dependancyInjector.LedgerService.AddJournal(
                    journal,
                    dependancyInjector.UserId,
                    dependancyInjector.CompanyId,
                    dependancyInjector.UserFullName, 
                    DateTimeOffset.UtcNow),
                "When you use Accounts Payable, you must choose a supplier in the Name field.");
        }

        [TestMethod]
        public async Task AddAccountsPayableJournalWithOrganisations()
        {
            //Create a journal
            //If ledger account type is not of type payable or receivable don't validate organisation
            Journal journal = new Journal
            {
                CurrencyId = 118,
                ExchangeRate = 1,
                Memo = "Simple journal with organisation reference",
                ReferenceNumber = "1",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 5,
                    Sequence = 1,
                    SubLedgerId = 9
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5050,
                    LedgerAccountId = 2,
                    Sequence = 2
                });
            Journal savedJournal = await dependancyInjector.LedgerService.AddJournal(
                journal,
                dependancyInjector.UserId,
                dependancyInjector.CompanyId,
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(savedJournal.Id > 0);
        }

        [TestMethod]
        public async Task AddAccountsPayableJournalWithMuiltiCurrency()
        {
            //Create a journal
            //If ledger account type is not of type payable or receivable don't validate organisation
            Journal journal = new Journal
            {
                CurrencyId = 6,
                ExchangeRate = 0.0091M,
                Memo = "A journal with organisation reference and multi currency",
                //ReferenceNumber = "2",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 6,
                    Sequence = 1,
                    SubLedgerId = 5
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5050,
                    LedgerAccountId = 2,
                    Sequence = 2
                });
            Journal savedJournal = await dependancyInjector.LedgerService.AddJournal(
                journal,
                dependancyInjector.UserId,
                dependancyInjector.CompanyId,
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(savedJournal.Id > 0);
        }

        [TestMethod]
        public async Task AddAccountsPayableJournalWithMuiltiCurrencyAndTax()
        {
            Journal journal = new Journal
            {
                CurrencyId = 118,
                ExchangeRate = 1,
                Memo = "A journal with organisation reference and multi currency and taxation",
                ReferenceNumber = "1",
                TransactionDate = DateTime.Now,
                TransactionType = Domains.Enums.TransactionType.GeneralJournal
            };

            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Debit = 5050,
                    LedgerAccountId = 6,
                    Sequence = 1,
                    SubLedgerId = 5,
                    TaxRateId = 2,
                    TaxAmount = 808
                });
            journal.JournalDetails.Add(
                new JournalDetail
                {
                    Credit = 5858,
                    LedgerAccountId = 2,
                    Sequence = 2
                });
            Journal savedJournal = await dependancyInjector.LedgerService.AddJournal(
                journal,
                dependancyInjector.UserId,
                dependancyInjector.CompanyId,
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(savedJournal.Id > 0);
        }

        [TestMethod]
        public async Task PostJournalWithMultiCurrency()
        {
            Journal journal = await dependancyInjector.LedgerService.PostJournal(
                15,
                dependancyInjector.UserId,
                dependancyInjector.UserFullName, 
                DateTimeOffset.UtcNow);
            Assert.IsTrue(journal.TransactionStatus == Domains.Enums.TransactionStatus.Posted);
        }

        //var obj = Task.Run(async () => await _messageService.SendEmail("remusmt@gmail.com", "Confirm your account", html)).GetAwaiter().GetResult();

    }
}
