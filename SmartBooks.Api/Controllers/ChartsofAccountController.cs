using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Api.Models;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View ChartsofAccount")]
    public class ChartsofAccountController : ControllerBase
    {
        private readonly LedgerAccountsService ledgerAccountsService;
        private readonly UserManager<ApplicationUser> userManager;
        public ChartsofAccountController(
            LedgerAccountsService accountsService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            ledgerAccountsService = accountsService;
        }

        [HttpGet("GetChartsofAccount")]
        public async Task<ActionResult<List<LedgerAccount>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await ledgerAccountsService.GetLedgerAccounts(user.CompanyId);
        }

        [HttpGet("GetPostingsAccounts")]
        public async Task<ActionResult<List<LedgerAccount>>> GetPostingsAccounts()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await ledgerAccountsService.GetPostingLedgerAccounts(user.CompanyId);
        }

        [Authorize(Policy = "Create ChartsofAccount")]
        [HttpPost]
        public async Task<ActionResult<LedgerAccount>> Post(LedgerAccountModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await ledgerAccountsService.Add(new LedgerAccount
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    AccountType = model.AccountType,
                    BankAccountNo = model.BankAccountNo,
                    DetailAccountType = model.DetailAccountType,
                    CurrencyId = model.CurrencyId,
                    HasOverDraft = model.HasOverDraft,
                    OverDraftLimit = model.OverDraftLimit,
                    ParentAccountId = model.ParentAccountId,
                    TaxRateId = model.TaxRateId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit ChartsofAccount")]
        [HttpPut]
        public async Task<ActionResult<LedgerAccount>> Put(LedgerAccountModel model)
        {
            LedgerAccount ledgerAccount = await ledgerAccountsService.GetLedgerAccountAsync(model.Id);
            if (ledgerAccount == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            if (ledgerAccount.UpdateCode > model.UpdateCode)
            {
                return BadRequest(new { new Exception("This record as been edited").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                ledgerAccount.Description = model.Description;
                ledgerAccount.AccountName = model.AccountName;
                ledgerAccount.AccountNumber = model.AccountNumber;
                ledgerAccount.AccountType = model.AccountType;
                ledgerAccount.BankAccountNo = model.BankAccountNo;
                ledgerAccount.DetailAccountType = model.DetailAccountType;
                ledgerAccount.CurrencyId = model.CurrencyId;
                ledgerAccount.HasOverDraft = model.HasOverDraft;
                ledgerAccount.OverDraftLimit = model.OverDraftLimit;
                ledgerAccount.ParentAccountId = model.ParentAccountId;
                ledgerAccount.TaxRateId = model.TaxRateId;

                return Ok(await ledgerAccountsService.Update(ledgerAccount, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete ChartsofAccount")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await ledgerAccountsService.Delete(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}