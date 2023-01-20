using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.UnitTests.DependencyManagement;
using System;
using System.Threading.Tasks;

namespace SmartBooks.UnitTests.Service_Tests
{
    [TestClass]
    public class MemberServiceTests
    {
        private readonly DependancyInjector dependancyInjector;
        public MemberServiceTests()
        {
            dependancyInjector = new DependancyInjector();
        }

        [TestMethod]
        public async Task AddingEmptyMember()
        {
            Member member = new Member();
            await Assert.ThrowsExceptionAsync<Exception>(
                async () => await dependancyInjector.MemberService.Add(
                    member,
                    dependancyInjector.UserId,
                    dependancyInjector.UserFullName, 
                    DateTime.UtcNow));
        }

        [TestMethod]
        public async Task AddingMemberWithoutNames()
        {
            Member member = new Member
            {
                CompanyId = dependancyInjector.CompanyId,
                ApplicationUserId = dependancyInjector.UserId
            };
            await Assert.ThrowsExceptionAsync<Exception>(
                async () => await dependancyInjector.MemberService.Add(
                    member,
                    dependancyInjector.UserId,
                    dependancyInjector.UserFullName, 
                    DateTime.UtcNow));
        }

        [TestMethod]
        public async Task AddingMember()
        {
            Member member = new Member
            {
                CompanyId = dependancyInjector.CompanyId,
                ApplicationUserId = dependancyInjector.UserId,
                LastName = "Muthomi"
            };
            Member member1 = await dependancyInjector.MemberService.Add(
                member,
                dependancyInjector.UserId,
                dependancyInjector.UserFullName, 
                DateTime.UtcNow);
            Assert.AreEqual(member1.Id, 1);
        }
    }
}
