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
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseService warehouseService;
        private readonly UserManager<ApplicationUser> userManager;
        public WarehouseController(
            WarehouseService warehouseServis,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            warehouseService = warehouseServis;
        }

        [HttpGet]
        public async Task<ActionResult<List<Warehouse>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await warehouseService.GetWarehouses(user.CompanyId);
        }

        [Authorize(Policy = "Create Warehouses")]
        [HttpPost]
        public async Task<ActionResult<Warehouse>> Post(WarehouseModel warehouse)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await warehouseService.AddWarehouse(new Warehouse
                {
                    Code = warehouse.Code,
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    DefaultDespatchBin = warehouse.DefaultDespatchBin,
                    DefaultReceivingBin = warehouse.DefaultReceivingBin,
                    Description = warehouse.Description
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
            
        }

        [Authorize(Policy = "Edit Warehouses")]
        [HttpPut]
        public async Task<ActionResult<Warehouse>> Put(WarehouseModel model)
        {
            Warehouse warehouse = await warehouseService.GetWarehouseAsync(model.Id);
            if (warehouse == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                warehouse.Code = model.Code;
                warehouse.DefaultDespatchBin = model.DefaultDespatchBin;
                warehouse.DefaultReceivingBin = model.DefaultReceivingBin;
                warehouse.Description = model.Description;
                return Ok(await warehouseService.UpdateWarehouse(warehouse, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
            
        }

        [Authorize(Policy = "Delete Warehouses")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await warehouseService.DeleteWarehouse(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

    }
}