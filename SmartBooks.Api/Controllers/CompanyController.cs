using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Settings")]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService companyService;
        private readonly UserManager<ApplicationUser> userManager;
        public CompanyController(
            CompanyService companyServici,
            UserManager<ApplicationUser> UserManager)
        {
            userManager = UserManager;
            companyService = companyServici;
        }

        [HttpGet("GetCompany")]
        public async Task<ActionResult<Company>> GetCompany()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await companyService.GetCompanyAsync(user.CompanyId);
        }

        [HttpGet("GetCountries")]
        [AllowAnonymous()]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            return await companyService.GetCountriesAsync();
        }

        [HttpGet("GetCurrencies")]
        [AllowAnonymous()]
        public async Task<ActionResult<List<Currency>>> GetCurrencies()
        {
            return await companyService.GetCurrenciesAsync();
        }

        [HttpGet("GetBanks/{countryId}")]
        [AllowAnonymous()]
        public async Task<ActionResult<List<Bank>>> GetBanks(int countryId)
        {
            return await companyService.GetBanksAsync(countryId);
        }

    }
}