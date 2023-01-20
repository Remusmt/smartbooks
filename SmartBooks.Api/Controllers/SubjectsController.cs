using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.ApplicationCore.SchoolServices;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SchoolEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly SubjectService subjectService;
        private readonly UserManager<ApplicationUser> userManager;

        public SubjectsController(
            SubjectService service,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            subjectService = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Subject>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await subjectService.GetSubjectsAsync(user.CompanyId);
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

        [Authorize(Policy = "Create Subjects")]
        [HttpPost]
        public async Task<ActionResult<Subject>> Post(Subject model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await subjectService.AddAsync(new Subject
                {
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

        [Authorize(Policy = "Edit Subjects")]
        [HttpPut]
        public async Task<ActionResult<Subject>> Put(Subject model)
        {
            Subject subject = await subjectService.GetSubjectAsync(model.Id);
            if (subject == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                subject.Description = model.Description;
                return Ok(await subjectService.UpdateAsync(
                    subject,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Subjects")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await subjectService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
