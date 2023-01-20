using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.UnitTests.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.UnitTests.Service_Tests
{
    [TestClass]
    public class MemberAccountServiceTests
    {
        private readonly DependancyInjector dependancyInjector;
        public MemberAccountServiceTests()
        {
            dependancyInjector = new DependancyInjector();
        }

        [TestMethod]
        public async Task AddFeesAccount()
        {
            MemberAccount memberAccount = await dependancyInjector.MemberAccountsService.AddFeesAccount(
                new ApplicationCore.Models.SaccoFeeAccountModel
                {
                    CompanyId = dependancyInjector.CompanyId,
                    CreatedBy = dependancyInjector.UserId,
                    CreatedByName = dependancyInjector.UserFullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    MemberAccountTypeId = 1,
                    MemberId = 1,
                    OpeningDate = DateTime.Now
                });

            Assert.IsTrue(memberAccount.Id > 0);
        }
    }
}
