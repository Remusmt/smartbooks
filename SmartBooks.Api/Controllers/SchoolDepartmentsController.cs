using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.ApplicationCore.SchoolServices;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SchoolEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Teaching Departments")]
    public class SchoolDepartmentsController : ControllerBase
    {
        private readonly TeachingDepartmentService departmentService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolDepartmentsController(
            TeachingDepartmentService service,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            departmentService = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<TeachingDepartment>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await departmentService.GetTeachingDepartmentsAsync(user.CompanyId);
        }

        [HttpGet("ExportExcel")]
        public async Task<ActionResult> ExportExcel()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return Ok();
        }

        [HttpGet("PrintPdf")]
        public async Task<ActionResult> PrintPdf()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return Ok();
        }

        [Authorize(Policy = "Create Teaching Departments")]
        [HttpPost]
        public async Task<ActionResult<TeachingDepartment>> Post(TeachingDepartment model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await departmentService.AddAsync(new TeachingDepartment
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    HoDId = model.HoDId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Teaching Departments")]
        [HttpPut]
        public async Task<ActionResult<TeachingDepartment>> Put(TeachingDepartment model)
        {
            TeachingDepartment department = await departmentService.GetTeachingDepartmentAsync(model.Id);
            if (department == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                department.Description = model.Description;
                department.HoDId = model.HoDId;
                return Ok(await departmentService.UpdateAsync(
                    department,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Teaching Departments")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await departmentService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
