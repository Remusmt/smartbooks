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
    [Authorize(Policy = "View Supplier Types")]
    public class SupplierTypesController : ControllerBase
    {
        private readonly CategoriesService<SupplierType> categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public SupplierTypesController(
            CategoriesService<SupplierType> supplierTypeService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            categoriesService = supplierTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierType>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await categoriesService.GetCategories(user.CompanyId);
        }

        [Authorize(Policy = "Create Supplier Types")]
        [HttpPost]
        public async Task<ActionResult<SupplierType>> Post(CategoryModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await categoriesService.AddCategory(new SupplierType
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

        [Authorize(Policy = "Edit Supplier Types")]
        [HttpPut]
        public async Task<ActionResult<SupplierType>> Put(CategoryModel model)
        {
            SupplierType category = await categoriesService.GetCategoryAsync(model.Id);
            if (category == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                category.Code = model.Code;
                category.Description = model.Description;
                return Ok(await categoriesService.UpdateCategory(category, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Supplier Types")]
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