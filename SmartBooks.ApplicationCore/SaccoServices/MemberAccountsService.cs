using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Models;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.SaccoServices
{
    public class MemberAccountsService
    {
        private readonly Logger logger;
        private readonly MemberService memberService;
        private readonly SaccoFeesService saccoFeesService;
        private readonly IMemberAccountRepository<MemberAccount> repository;

        public MemberAccountsService(
            Logger loger,
            MemberService memberSer,
            SaccoFeesService saccoFeesSer,
            IMemberAccountRepository<MemberAccount> repo
            )
        {
            logger = loger;
            repository = repo;
            memberService = memberSer;
            saccoFeesService = saccoFeesSer;
        }

        public async Task<MemberAccount> GetMemberAccountAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<MemberAccount>> GetMemberAccountsAsync(int companyId)
        {
            return await repository
                .ListAsync(new MemberAccountSpecification(companyId));
        }

        public async Task<MemberAccount> AddFeesAccount(SaccoFeeAccountModel feeAccountModel)
        {
            Member member = await memberService.GetMemberAsync(feeAccountModel.MemberId);
            if (member == null) throw new Exception("Member not found");

            SaccoFee saccoFee = await saccoFeesService.GetSaccoFeeAsync(feeAccountModel.MemberAccountTypeId);
            if (saccoFee == null) throw new Exception("Fees type not found");

            MemberAccount memberAccount = new MemberAccount
            {
                AccountNumber = member.MemberNumber,
                CompanyId = feeAccountModel.CompanyId,
                CreatedBy = feeAccountModel.CreatedBy,
                CreatedByName = feeAccountModel.CreatedByName,
                CreatedOn = feeAccountModel.CreatedOn,
                MemberAccountTypeId = feeAccountModel.MemberAccountTypeId,
                MemberId = feeAccountModel.MemberId,
                OpeningDate = feeAccountModel.OpeningDate,
                AccountStatus = MemberAccountStatus.Active
            };

            repository.Add(memberAccount);
            await repository.SaveChangesAsync();

            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = memberAccount.CompanyId,
                CreatedBy = memberAccount.CreatedBy,
                CreatedByName = memberAccount.CreatedByName,
                CreatedOn = memberAccount.CreatedOn,
                RecordId = memberAccount.Id,
                RecordType = RecordType.MemberAccount,
                SerializedRecord = logger.SeliarizeObject(memberAccount)
            });

            return memberAccount;
        }

        public async Task<MemberAccount> SuspendAccount(
            int accountId,
            int companyId,
            int userId,
            string userName,
            DateTimeOffset suspendedOn)
        {
            MemberAccount memberAccount = await repository.GetByIdAsync(accountId);
            if (memberAccount == null) throw new Exception("Invalid account");

            memberAccount.AccountStatus = MemberAccountStatus.Suspended;
            await repository.Update(memberAccount);

            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = companyId,
                CreatedBy = userId,
                CreatedByName = userName,
                CreatedOn = suspendedOn,
                RecordId = memberAccount.Id,
                RecordType = RecordType.MemberAccount,
                SerializedRecord = logger.SeliarizeObject(memberAccount)
            });

            return memberAccount;
        }

    }
}
