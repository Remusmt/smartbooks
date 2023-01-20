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
    [Authorize(Policy = "View Class Rooms")]
    public class ClassRoomsController : ControllerBase
    {
        private readonly ClassRoomService classRoomService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClassRoomsController(
            ClassRoomService classRoomSer,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            classRoomService = classRoomSer;
        }
        [HttpGet]
        public async Task<ActionResult<List<ClassRoom>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await classRoomService.GetClassRoomsAsync(user.CompanyId);
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

        [Authorize(Policy = "Create Class Rooms")]
        [HttpPost]
        public async Task<ActionResult<ClassRoom>> Post(ClassRoom model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await classRoomService.AddAsync(new ClassRoom
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    BlockId = model.BlockId,
                    Capacity = model.Capacity,
                    LevelId = model.LevelId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Class Rooms")]
        [HttpPut]
        public async Task<ActionResult<ClassRoom>> Put(ClassRoom model)
        {
            ClassRoom classRoom = await classRoomService.GetClassRoomAsync(model.Id);
            if (classRoom == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                classRoom.Description = model.Description;
                classRoom.BlockId = model.BlockId;
                classRoom.Capacity = model.Capacity;
                classRoom.LevelId = model.LevelId;
                return Ok(await classRoomService.UpdateAsync(
                    classRoom,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Class Rooms")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await classRoomService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
