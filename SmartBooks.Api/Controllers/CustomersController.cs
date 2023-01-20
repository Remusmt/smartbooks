using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Api.Models;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Models;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly OrganisationsServices<Customer> service;
        private readonly UserManager<ApplicationUser> userManager;

        public CustomersController(
            OrganisationsServices<Customer> organisationsServices,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            service = organisationsServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetOrganisations(user.CompanyId);
        }

        [HttpGet("{sort}/{order}/{page}/{pageSize}")]
        public async Task<ActionResult<OrganisationModel<Customer>>>
            Get(string sort, string order, int page, int pageSize)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetOrganisations(user.CompanyId, sort, order, page, pageSize);
        }

        [Authorize(Policy = "Create Customers")]
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(CustomerModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.Add(new Customer
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CategoryId = model.CategoryId,
                    CreditLimit = model.CreditLimit,
                    CreditLimitPeriod = model.CreditLimitPeriod,
                    CurrencyId = model.CurrencyId,
                    DefaultAddressId = model.DefaultAddressId,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    IdNumber = model.IdNumber,
                    LastName = model.LastName,
                    LedgerAccountId = model.LedgerAccountId,
                    Name = model.Name,
                    OrganisationType = Domains.Enums.OrganisationType.Customer,
                    PaymentTermId = model.PaymentTermId,
                    PhoneNumber = model.PhoneNumber
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Customers")]
        [HttpPut]
        public async Task<ActionResult<Customer>> Put(CustomerModel model)
        {
            Customer customer = await service.GetOrganisationAsync(model.Id);
            if (customer == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                customer.CategoryId = model.CategoryId;
                customer.CreditLimit = model.CreditLimit;
                customer.CreditLimitPeriod = model.CreditLimitPeriod;
                customer.CurrencyId = model.CurrencyId;
                customer.DefaultAddressId = model.DefaultAddressId;
                customer.Email = model.Email;
                customer.FirstName = model.FirstName;
                customer.IdNumber = model.IdNumber;
                customer.LastName = model.LastName;
                customer.LedgerAccountId = model.LedgerAccountId;
                customer.Name = model.Name;
                customer.PaymentTermId = model.PaymentTermId;
                customer.PhoneNumber = model.PhoneNumber;
                return Ok(await service.Update(customer, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Customers")]
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

        [Authorize(Policy = "Manage Customer Addresses")]
        [HttpPost("CreateAddress")]
        public async Task<ActionResult<OrganisationAddress>> CreateAddress(OrganisationAddress model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.CreateAddress(new OrganisationAddress
                {
                    CountryId = model.CountryId,
                    City = model.City,
                    Location = model.Location,
                    OrganisationId = model.OrganisationId,
                    PoBox = model.PoBox,
                    PostalCode = model.PostalCode
                }, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage Customer Addresses")]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<OrganisationAddress>> UpdateAddress(OrganisationAddress model)
        {
            OrganisationAddress address = await service.GetOrganisationAddressAsync(model.Id);
            if (address == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                address.City = model.City;
                address.CountryId = model.CountryId;
                address.Location = model.Location;
                address.PoBox = model.PoBox;
                address.PostalCode = model.PostalCode;

                return Ok(await service.UpdateAddress(address, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage Customer Addresses")]
        [HttpDelete("DeleteAddress/{id}")]
        public async Task<ActionResult<int>> DeleteAddress(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await service.DeleteAddress(id, user.Id, user.CompanyId, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}