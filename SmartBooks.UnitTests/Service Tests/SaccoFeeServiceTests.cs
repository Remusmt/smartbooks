using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.UnitTests.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.UnitTests.Service_Tests
{
    [TestClass]
    public class SaccoFeeServiceTests
    {
        private readonly DependancyInjector dependancyInjector;
        public SaccoFeeServiceTests()
        {
            dependancyInjector = new DependancyInjector();
        }

        [TestMethod]
        public async Task AddSaccoFeeTest()
        {
            SaccoFee saccoFee = new SaccoFee
            {
                Amount = 1000,
                CompanyId = dependancyInjector.CompanyId,
                CreatedBy = dependancyInjector.UserId,
                CreatedByName = dependancyInjector.UserFullName,
                CreatedOn = DateTimeOffset.UtcNow,
                SaccoFeesType = SaccoFeesType.Registration,
                Description = "Membership Fees"
            };
            saccoFee = await dependancyInjector.SaccoFeesService.Add(saccoFee);
            Assert.IsTrue(saccoFee.Id > 1);
        }

        [TestMethod]
        public async Task AddSaccoUserDefinedFee()
        {
            SaccoFee saccoFee = new SaccoFee
            {
                CompanyId = dependancyInjector.CompanyId,
                CreatedBy = dependancyInjector.UserId,
                CreatedByName = dependancyInjector.UserFullName,
                CreatedOn = DateTimeOffset.UtcNow,
                SaccoFeesType = SaccoFeesType.UserDefined,
                Description = "Management Fees"
            };
            saccoFee = await dependancyInjector.SaccoFeesService.Add(saccoFee);
            Assert.IsTrue(saccoFee.Id > 1);
        }

        [TestMethod]
        public async Task UpdateSaccoFee()
        {
            SaccoFee saccoFee = await dependancyInjector.SaccoFeesService.GetSaccoFeeAsync(5);
            saccoFee.Description = "Management Fee";
            saccoFee = await dependancyInjector.SaccoFeesService
                .Update(saccoFee, dependancyInjector.UserId, dependancyInjector.UserFullName, DateTimeOffset.UtcNow);
            Assert.AreEqual(saccoFee.Description, "Management Fee");
        }

    }
}
