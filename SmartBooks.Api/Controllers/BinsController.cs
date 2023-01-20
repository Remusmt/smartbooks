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
    [Authorize(Policy = "View Warehouses")]
    public class BinsController : ControllerBase
    {
        private readonly WarehouseService warehouseService;
        private readonly UserManager<ApplicationUser> userManager;
        public BinsController(
            WarehouseService warehouseServis,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            warehouseService = warehouseServis;
        }

        [HttpGet("{warehouseId}")]
        public async Task<ActionResult<List<Bin>>> Get(int warehouseId)
        {
            return await warehouseService.GetBins(warehouseId);
        }

        [Authorize(Policy = "Manage Bins")]
        [HttpPost]
        public async Task<ActionResult<Warehouse>> Post(BinModel bin)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await warehouseService.AddBin(new Bin
                {
                    Code = bin.Code,
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = bin.Description,
                    WarehouseId = bin.WarehouseId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage Bins")]
        [HttpPut]
        public async Task<ActionResult<Warehouse>> Put(BinModel model)
        {
            Bin bin = await warehouseService.GetBinAsync(model.Id);
            if (bin == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                bin.Code = model.Code;
                bin.Description = model.Description;
                bin.WarehouseId = model.WarehouseId;
                return Ok(await warehouseService.UpdateBin(bin, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Manage Bins")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await warehouseService.DeleteBin(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}