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
    [Authorize(Policy = "View Taxes")]
    public class TaxesController : ControllerBase
    {
        private readonly TaxesService service;
        private readonly UserManager<ApplicationUser> userManager;
        public TaxesController(
            TaxesService taxService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            service = taxService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tax>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetTaxes(user.CompanyId);
        }

        [HttpGet("GetTaxRates")]
        public async Task<ActionResult<List<TaxRate>>> GetTaxRates()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetTaxRates(user.CompanyId);
        }

        [Authorize(Policy = "Create Taxes")]
        [HttpPost]
        public async Task<ActionResult<Tax>> Post(TaxModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.Add(new Tax
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    Code = model.Code,
                    PurchasesAccountId = model.PurchasesAccountId,
                    ReportingMethod = model.ReportingMethod,
                    SalesAccountId = model.SalesAccountId,
                    TaxAgencyId = model.TaxAgencyId,
                    TaxRegistrationNumber = model.TaxRegistrationNumber
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Taxes")]
        [HttpPut]
        public async Task<ActionResult<Tax>> Put(TaxModel model)
        {
            Tax tax = await service.GetTaxAsync(model.Id);
            if (tax == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                tax.Description = model.Description;
                tax.Code = model.Code;
                tax.PurchasesAccountId = model.PurchasesAccountId;
                tax.ReportingMethod = model.ReportingMethod;
                tax.SalesAccountId = model.SalesAccountId;
                tax.TaxAgencyId = model.TaxAgencyId;
                tax.TaxRegistrationNumber = model.TaxRegistrationNumber;
                return Ok(await service.Update(tax, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Taxes")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await service.Delete(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Policy = "Manage TaxRate")]
        [HttpPost("CreateTaxRate")]
        public async Task<ActionResult<TaxRate>> CreateTaxRate(TaxRate model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.CreateTaxRate(new TaxRate
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    PurchaseRate = model.PurchaseRate,
                    PurchaseTaxIsReclaimable = model.PurchaseTaxIsReclaimable,
                    SalesRate = model.SalesRate,
                    TaxId = model.TaxId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage TaxRate")]
        [HttpPut("UpdateTaxRate")]
        public async Task<ActionResult<TaxRate>> UpdateTaxRate(TaxRate model)
        {
            TaxRate taxRate = await service.GetTaxRateAsync(model.Id);
            if (taxRate == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                taxRate.Description = model.Description;
                taxRate.PurchaseRate = model.PurchaseRate;
                taxRate.PurchaseTaxIsReclaimable = model.PurchaseTaxIsReclaimable;
                taxRate.SalesRate = model.SalesRate;

                return Ok(await service.UpdateTaxRate(taxRate, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage TaxRate")]
        [HttpDelete("DeleteTaxRate/{id}")]
        public async Task<ActionResult<int>> DeleteTaxRate(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await service.DeleteTaxRate(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

    }
}