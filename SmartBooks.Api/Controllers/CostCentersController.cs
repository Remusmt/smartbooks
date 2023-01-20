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
    [Authorize(Policy = "View CostCenters")]
    public class CostCentersController : ControllerBase
    {
        private readonly CategoriesService<CostCenter> categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public CostCentersController(
            CategoriesService<CostCenter> costCenterService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            categoriesService = costCenterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CostCenter>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await categoriesService.GetCategories(user.CompanyId);
        }

        [Authorize(Policy = "Create CostCenters")]
        [HttpPost]
        public async Task<ActionResult<CostCenter>> Post(CategoryModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await categoriesService.AddCategory(new CostCenter
                {
                    Code = model.Code,
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit CostCenters")]
        [HttpPut]
        public async Task<ActionResult<CostCenter>> Put(CategoryModel model)
        {
            CostCenter costCenter = await categoriesService.GetCategoryAsync(model.Id);
            if (costCenter == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                costCenter.Code = model.Code;
                costCenter.Description = model.Description;
                return Ok(await categoriesService.UpdateCategory(costCenter, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message});
            }

        }

        [Authorize(Policy = "Delete CostCenters")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await categoriesService.DeleteCategory(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}