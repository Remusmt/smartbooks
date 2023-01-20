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
    [Authorize(Policy = "View Suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService service;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersController(
            SupplierService organisationsServices,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            service = organisationsServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetOrganisations(user.CompanyId);
        }

        [HttpGet("GetTaxAngencies")]
        public async Task<ActionResult<List<Supplier>>> GetTaxAngencies()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetTaxAngencies(user.CompanyId);
        }

        [HttpGet("{sort}/{order}/{page}/{pageSize}")]
        public async Task<ActionResult<SupplierListModel>>
           Get(string sort, string order, int page, int pageSize)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetOrganisations(user.CompanyId, sort, order, page, pageSize);
        }

        [Authorize(Policy = "Create Suppliers")]
        [HttpPost]
        public async Task<ActionResult<Supplier>> Post(SupplierModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.Add(new Supplier
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
                    OrganisationType = Domains.Enums.OrganisationType.Supplier,
                    PaymentTermId = model.PaymentTermId,
                    PhoneNumber = model.PhoneNumber,
                    BankId = model.BankId,
                    BankBranchId = model.BankBranchId,
                    BankAccountNumber = model.BankAccountNumber,
                    IsTaxAgency = model.IsTaxAgency
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Suppliers")]
        [HttpPut]
        public async Task<ActionResult<Supplier>> Put(SupplierModel model)
        {
            Supplier supplier = await service.GetOrganisationAsync(model.Id);
            if (supplier == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                supplier.CategoryId = model.CategoryId;
                supplier.CreditLimit = model.CreditLimit;
                supplier.CreditLimitPeriod = model.CreditLimitPeriod;
                supplier.CurrencyId = model.CurrencyId;
                supplier.DefaultAddressId = model.DefaultAddressId;
                supplier.Email = model.Email;
                supplier.FirstName = model.FirstName;
                supplier.IdNumber = model.IdNumber;
                supplier.LastName = model.LastName;
                supplier.LedgerAccountId = model.LedgerAccountId;
                supplier.Name = model.Name;
                supplier.PaymentTermId = model.PaymentTermId;
                supplier.PhoneNumber = model.PhoneNumber;
                supplier.BankId = model.BankId;
                supplier.BankBranchId = model.BankBranchId;
                supplier.BankAccountNumber = model.BankAccountNumber;
                supplier.IsTaxAgency = model.IsTaxAgency;
                return Ok(await service.Update(supplier, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Suppliers")]
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

        [Authorize(Policy = "Manage Supplier Addresses")]
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

        [Authorize(Policy = "Manage Supplier Addresses")]
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

        [Authorize(Policy = "Manage Supplier Addresses")]
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