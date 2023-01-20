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
    [Authorize(Policy = "View Dormitory")]
    public class DormitoriesController : ControllerBase
    {
        private readonly DormitoryService dormitoryServiceService;
        private readonly UserManager<ApplicationUser> userManager;

        public DormitoriesController(
            DormitoryService service,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            dormitoryServiceService = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Dormitory>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await dormitoryServiceService.GetDormitoriesAsync(user.CompanyId);
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

        [Authorize(Policy = "Create Dormitory")]
        [HttpPost]
        public async Task<ActionResult<Dormitory>> Post(Dormitory model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await dormitoryServiceService.AddAsync(new Dormitory
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    Capacity = model.Capacity
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Dormitory")]
        [HttpPut]
        public async Task<ActionResult<Dormitory>> Put(Dormitory model)
        {
            Dormitory dormitory = await dormitoryServiceService.GetDormitoryAsync(model.Id);
            if (dormitory == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                dormitory.Description = model.Description;
                dormitory.Capacity = model.Capacity;
                return Ok(await dormitoryServiceService.UpdateAsync(
                    dormitory,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Dormitory")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await dormitoryServiceService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
