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
    [Authorize(Policy = "View Levels")]
    public class SchoolLevelsController : ControllerBase
    {
        private readonly LevelService levelService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolLevelsController(
            LevelService levelSer,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            levelService = levelSer;
        }
        [HttpGet]
        public async Task<ActionResult<List<Level>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await levelService.GetLevelsAsync(user.CompanyId);
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

        [Authorize(Policy = "Create Levels")]
        [HttpPost]
        public async Task<ActionResult<Level>> Post(Level model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await levelService.AddAsync(new Level
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    SchoolLevel = model.SchoolLevel
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Levels")]
        [HttpPut]
        public async Task<ActionResult<Level>> Put(Level model)
        {
            Level level = await levelService.GetLevelAsync(model.Id);
            if (level == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                level.Description = model.Description;
                level.SchoolLevel = model.SchoolLevel;
                return Ok(await levelService.UpdateAsync(
                    level,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Levels")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await levelService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
