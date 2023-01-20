using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Api.Models;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Models;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Units of Measure")]
    public class UnitsofMeasureController : ControllerBase
    {
        private readonly UnitsofMeasureService service;
        private readonly UserManager<ApplicationUser> userManager;
        public UnitsofMeasureController(
            UnitsofMeasureService measureService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            service = measureService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UnitofMeasureListModel>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetUnitsofMeasure(user.CompanyId);
        }

        [Authorize(Policy = "Create Units of Measure")]
        [HttpPost]
        public async Task<ActionResult<UnitofMeasureListModel>> Post(UnitofMeasureModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.Add(new UnitofMeasure
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    Abbreviation = model.Abbreviation,
                    Type = model.Type
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Units of Measure")]
        [HttpPut]
        public async Task<ActionResult<UnitofMeasureListModel>> Put(UnitofMeasureModel model)
        {
            UnitofMeasure unitofMeasure = await service.GetUnitofMeasureAsync(model.Id);
            if (unitofMeasure == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                unitofMeasure.Description = model.Description;
                unitofMeasure.Abbreviation = model.Abbreviation;
                unitofMeasure.Type = model.Type;
                return Ok(await service.Update(unitofMeasure, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Units of Measure")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await service.Delete(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Policy = "Manage Unit Conversions")]
        [HttpPost("CreateConversion")]
        public async Task<ActionResult<UomConversionModel>> CreateConversion(UomConversion model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.AddConversion(new UomConversion
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    Factor = model.Factor,
                    UnitofMeasureFromId = model.UnitofMeasureFromId,
                    UnitofMeasureToId = model.UnitofMeasureToId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Policy = "Manage Unit Conversions")]
        [HttpPut("UpdateConversion")]
        public async Task<ActionResult<UomConversionModel>> UpdateConversion(UomConversion model)
        {
            UomConversion uomConversion = await service.GetUomConversionAsync(model.Id);
            if (uomConversion == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                uomConversion.Description = model.Description;
                uomConversion.Factor = model.Factor;
                uomConversion.UnitofMeasureFromId = model.UnitofMeasureFromId;
                uomConversion.UnitofMeasureToId = model.UnitofMeasureToId;
                return Ok(await service.UpdateConversion(uomConversion, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Policy = "Manage Unit Conversions")]
        [HttpDelete("DeleteConversion/{id}")]
        public async Task<ActionResult<int>> DeleteConversion(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await service.DeleteConversion(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}