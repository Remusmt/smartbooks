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
    [Authorize(Policy = "View Utility Rooms")]
    public class UtilityRoomsController : ControllerBase
    {
        private readonly UtilityRoomService utilityRoomService;
        private readonly UserManager<ApplicationUser> userManager;

        public UtilityRoomsController(
            UtilityRoomService service,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            utilityRoomService = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<UtilityRoom>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await utilityRoomService.GetUtilityRoomsAsync(user.CompanyId);
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

        [Authorize(Policy = "Create Utility Rooms")]
        [HttpPost]
        public async Task<ActionResult<UtilityRoom>> Post(UtilityRoom model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await utilityRoomService.AddAsync(new UtilityRoom
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

        [Authorize(Policy = "Edit Utility Rooms")]
        [HttpPut]
        public async Task<ActionResult<UtilityRoom>> Put(UtilityRoom model)
        {
            UtilityRoom utilityRoom = await utilityRoomService.GetUtilityRoomAsync(model.Id);
            if (utilityRoom == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                utilityRoom.Description = model.Description;
                return Ok(await utilityRoomService.UpdateAsync(
                    utilityRoom,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Utility Rooms")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await utilityRoomService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
