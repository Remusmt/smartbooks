using System;
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
    [Authorize(Policy = "View Inventory Items")]
    public class InventoryItemsController : ControllerBase
    {
        private readonly InventoryItemService service;
        private readonly UserManager<ApplicationUser> userManager;
        public InventoryItemsController(
            InventoryItemService itemService,
            UserManager<ApplicationUser> userMan)
        {
            userManager = userMan;
            service = itemService;
        }

        [HttpGet("{sort}/{order}/{page}/{pageSize}")]
        public async Task<ActionResult<InventoryItemsListModel>>
            Get(string sort, string order, int page, int pageSize)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await service.GetInventoryItems(user.CompanyId, sort, order, page, pageSize);
        }

        [Authorize(Policy = "Create Inventory Items")]
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> Post(InventoryItemModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await service.Add(new InventoryItem
                {
                    CompanyId = user.CompanyId,
                    CreatedBy = user.Id,
                    CreatedByName = user.FullName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Description = model.Description,
                    Code = model.Code,
                    AssetAcount = model.AssetAcount,
                    CogsAccount = model.CogsAccount,
                    IncomeAccount = model.IncomeAccount,
                    InventoryCategoryId = model.InventoryCategoryId,
                    StandardCost = model.StandardCost,
                    StandardPrice = model.StandardPrice,
                    TaxId = model.TaxId,
                    Type = model.Type,
                    UnitofMeasureId = model.UnitofMeasureId
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Edit Inventory Items")]
        [HttpPut]
        public async Task<ActionResult<InventoryItem>> Put(InventoryItemModel model)
        {
            InventoryItem inventoryItem = await service.GetInventoryItemAsync(model.Id);
            if (inventoryItem == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                inventoryItem.Description = model.Description;
                inventoryItem.Code = model.Code;
                inventoryItem.AssetAcount = model.AssetAcount;
                inventoryItem.CogsAccount = model.CogsAccount;
                inventoryItem.IncomeAccount = model.IncomeAccount;
                inventoryItem.InventoryCategoryId = model.InventoryCategoryId;
                inventoryItem.StandardCost = model.StandardCost;
                inventoryItem.StandardPrice = model.StandardPrice;
                inventoryItem.TaxId = model.TaxId;
                //inventoryItem.Type = model.Type;
                inventoryItem.UnitofMeasureId = model.UnitofMeasureId;
                return Ok(await service.Update(inventoryItem, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Inventory Items")]
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
    }
}