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
    [Authorize(Policy = "View PaymentTerms")]
    public class PaymentTermsController : ControllerBase
    {
        private readonly PaymentTermService paymentTermService;
        private readonly UserManager<ApplicationUser> userManager;
        public PaymentTermsController(
            PaymentTermService termService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            paymentTermService = termService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentTerm>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await paymentTermService.GetPaymentTerms(user.CompanyId);
        }

        [Authorize(Policy = "Create PaymentTerms")]
        [HttpPost]
        public async Task<ActionResult<PaymentTerm>> Post(PaymentTermModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await paymentTermService.Add(new PaymentTerm
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    DateDriven = model.DateDriven,
                    DiscountIfPaidBefore = model.DiscountIfPaidBefore,
                    DiscountIfPaidWithin = model.DiscountIfPaidWithin,
                    DiscountPercentage = model.DiscountPercentage,
                    DueNextMonthIfIssued = model.DueNextMonthIfIssued,
                    NetDueBefore = model.NetDueBefore,
                    NetDueIn = model.NetDueIn
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit PaymentTerms")]
        [HttpPut]
        public async Task<ActionResult<PaymentTerm>> Put(PaymentTermModel model)
        {
            PaymentTerm paymentTerm = await paymentTermService.GetPaymentTermAsync(model.Id);
            if (paymentTerm == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                paymentTerm.Description = model.Description;
                paymentTerm.DateDriven = model.DateDriven;
                paymentTerm.DiscountIfPaidBefore = model.DiscountIfPaidBefore;
                paymentTerm.DiscountIfPaidWithin = model.DiscountIfPaidWithin;
                paymentTerm.DiscountPercentage = model.DiscountPercentage;
                paymentTerm.DueNextMonthIfIssued = model.DueNextMonthIfIssued;
                paymentTerm.NetDueBefore = model.NetDueBefore;
                paymentTerm.NetDueIn = model.NetDueIn;
                return Ok(await paymentTermService.Update(paymentTerm, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete PaymentTerms")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await paymentTermService.Delete(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}