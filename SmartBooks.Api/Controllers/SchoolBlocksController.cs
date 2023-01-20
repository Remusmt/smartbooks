using DinkToPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Api.Helpers;
using SmartBooks.Api.Helpers.Models;
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
    //[Authorize(Policy = "View Blocks")]
    public class SchoolBlocksController : ControllerBase
    {
        private readonly HtmlHelper htmlHelper;
        private readonly BlockService blockService;
        private readonly SynchronizedConverter converter;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolBlocksController(
            HtmlHelper html_Helper,
            BlockService blockSer,
            SynchronizedConverter converta,
            UserManager<ApplicationUser> userMan)
        {
            converter = converta;
            htmlHelper = html_Helper;
            userManager = userMan;
            blockService = blockSer;
        }
        [HttpGet]
        public async Task<ActionResult<List<Block>>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await blockService.GetBlocksAsync(user.CompanyId);
        }

        [HttpGet("ExportExcel")]
        public FileResult ExportExcel()
        {
            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            List<Block> blocks = new List<Block>
            {
                new Block
                {
                    Description = "Test",
                    CreatedByName = "Remus",
                    CreatedBy = 2
                },
                new Block
                {
                    Description = "Test Two",
                    UpdateCode = 3
                },
            };
            List<ColumnModel> headers = new List<ColumnModel>
            {
                new ColumnModel { ColumnName = "Description", Title = "Description" , Type = Type.GetType("System.String")},
                new ColumnModel { ColumnName = "CreatedByName", Title = "Created By", Type = Type.GetType("System.String")}
            };
            return File(FileHelpers.ExportTabularDataToExcel(blocks, headers, "Blocks"),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Blocks.xlsx"); ;
        }

        [HttpGet("PrintPdf")]
        public async  Task<FileResult> PrintPdf()
        {
            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            List<string> descriptions = new List<string>
            {
                "South Block",
                "North Block",
                "East Block"
            };
            var response = await htmlHelper
                .GetTemplateHtmlAsStringAsync(
                "~/Templates/SingleColumn.cshtml", descriptions);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
                Objects = {
                  new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = response,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    //HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                  }
                }
            };
            byte[] pdf = converter.Convert(doc);
            return File(pdf, "application/pdf");
        }

        [Authorize(Policy = "Create Blocks")]
        [HttpPost]
        public async Task<ActionResult<Block>> Post(Block model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(await blockService.AddAsync(new Block
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

        [Authorize(Policy = "Edit Blocks")]
        [HttpPut]
        public async Task<ActionResult<Block>> Put(Block model)
        {
            Block block = await blockService.GetBlockAsync(model.Id);
            if (block == null)
            {
                return NotFound(new { new Exception("Record not found").Message });
            }
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                block.Description = model.Description;
                return Ok(await blockService.UpdateAsync(
                    block,
                    user.Id,
                    user.FullName,
                    DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }

        [Authorize(Policy = "Delete Blocks")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                return Ok(await blockService.DeleteAsync(id, user.Id, user.FullName, DateTimeOffset.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
