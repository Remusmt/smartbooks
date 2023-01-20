using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.ApplicationCore.SaccoServices;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "View Members")]
    public class MemberController : ControllerBase
    {
        private readonly MemberService memberService;
        private readonly UserManager<ApplicationUser> userManager;
        public MemberController(
            MemberService service,
            UserManager<ApplicationUser> manager)
        {
            memberService = service;
            userManager = manager;
        }

        [Authorize(Policy = "Create Members")]
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return await memberService.Add(member, user.Id, user.FullName, DateTime.UtcNow);
        }
    }
}
