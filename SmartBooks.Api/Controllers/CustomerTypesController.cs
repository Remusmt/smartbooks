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
    [Authorize(Policy = "View Customer Types")]
    public class CustomerTypesController : ControllerBase
    {
        private readonly CategoriesService<CustomerType> categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public CustomerTypesController(
            CategoriesService<CustomerType> customerTypeService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            categoriesService = customerTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerType>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await categoriesService.GetCategories(user.CompanyId);
        }

        [Authorize(Policy = "Create Customer Types")]
        [HttpPost]
        public async Task<ActionResult<CustomerType>> Post(CategoryModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await categoriesService.AddCategory(new CustomerType
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

        [Authorize(Policy = "Edit Customer Types")]
        [HttpPut]
        public async Task<ActionResult<CustomerType>> Put(CategoryModel model)
        {
            CustomerType category = await categoriesService.GetCategoryAsync(model.Id);
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

        [Authorize(Policy = "Delete Customer Types")]
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