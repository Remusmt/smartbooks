using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartBooks.Api.Helpers;
using SmartBooks.Api.Models;
using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;

namespace SmartBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly Logger logger;
        private readonly AppSettings appSettings;
        private readonly CompanyService companyService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountsController(
            Logger loger,
            CompanyService CompanyService,
            IOptions<AppSettings> AppSettings,
            UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> SignInManager)
        {
            logger = loger;
            userManager = UserManager;
            signInManager = SignInManager;
            appSettings = AppSettings.Value;
            companyService = CompanyService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(model);
                }

                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        return BadRequest(new { error = "Confirm your email" });
                    }
                }
                else
                {
                    return BadRequest(new { error = "Invalid login attempt." });
                }

                Microsoft.AspNetCore.Identity.SignInResult result;
                try
                {
                    result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                }
                catch (Exception ec)
                {

                    return StatusCode(500, new { error = $"{ec.Message} error while login" });
                }

                if (result.Succeeded)
                {
                    //Generate token
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    };
                    claims.AddRange(await userManager.GetClaimsAsync(user));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
                    var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                    var tokenString = new JwtSecurityToken(
                         issuer: "witsoft.co.ke",
                         audience: "witsoft.co.ke",
                         expires: DateTime.Now.AddDays(15),
                         claims: claims,
                         signingCredentials: signingCred
                        );
                    if (user.CompanyId == 0)
                    {
                        return StatusCode(500, new { error = "Company not found" });
                    }

                    var company = await companyService.GetCompanyAsync(user.CompanyId);
                    if (company == null)
                    {
                        return StatusCode(500, new { error = "Company not found" });
                    }
                    var defaults = await companyService.GetCompanyDefaultsAsync(user.CompanyId);
                    if (defaults == null)
                    {
                        return StatusCode(500, new { error = "Company settings not found" });
                    }

                    LoggedInUserViewModel loggedInUser = new LoggedInUserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        CompanyId = user.CompanyId,
                        CompanyName = company.CompanyName,
                        TokenString = new JwtSecurityTokenHandler().WriteToken(tokenString),
                        Succeeded = true,
                        CanManageUsers = claims.Any(e => e.Type == "View Users" && e.Value == "true"),
                        Menus = claims.GetMenus(company.CompanyType),
                        CompanyDefault = new CompanyDefault
                        {
                            AllowPostingToParentAccount = defaults.AllowPostingToParentAccount,
                            DefaultCurrency = defaults.DefaultCurrency,
                            DefaultWarehouse = defaults.DefaultWarehouse,
                            EnableInventoryTracking = defaults.EnableInventoryTracking,
                            HasMultipleWarehouses = defaults.HasMultipleWarehouses,
                            UseAccountNumbers = defaults.UseAccountNumbers
                        }
                    };

                    await logger.Log(new AuditLog
                    {
                        ActionType = ActionType.Login,
                        CompanyId = loggedInUser.CompanyId,
                        CreatedBy = loggedInUser.Id,
                        CreatedByName = loggedInUser.FullName,
                        CreatedOn = DateTimeOffset.UtcNow,
                        LogType = LogType.Info,
                        RecordType = RecordType.User,
                        SerializedRecord = $"{loggedInUser.FullName} logged in at {DateTime.Now.ToString("dd MM yyyy")}"
                    });

                    return Ok(loggedInUser);
                }
                if (result.RequiresTwoFactor)
                {
                    return BadRequest(new { error = "You are a miracle worker." });
                }
                if (result.IsLockedOut)
                {
                    return BadRequest(new { error = "Account locked out." });
                }
                else
                {
                    return BadRequest(new { error = "Invalid username or password." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CountryId == 0)
                {
                    return BadRequest(new { new Exception("Invalid country").Message });
                }

                var company = await companyService.Register(model.CompanyName, model.CountryId, DateTimeOffset.UtcNow);

                if (company.Id == 0)
                {
                    return StatusCode(500, new { error = "Error creating company" });
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CompanyId = company.Id
                };
                try
                {
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var registeredUser = await userManager.FindByNameAsync(model.Email);

                        // Generate validation code
                        string code = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);

                        result = await userManager.ConfirmEmailAsync(registeredUser, code);

                        bool isAdmin = await userManager.Users
                            .AnyAsync(e => e.CompanyId == company.Id);

                        await userManager.AddClaimsAsync(registeredUser, UserHelper.GetClaims(!isAdmin));

                        if (result.Succeeded)
                        {
                            List<Claim> claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName)
                            };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
                            var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                            var tokenString = new JwtSecurityToken(
                                 issuer: "witsoft.co.ke",
                                 expires: DateTime.Now.AddDays(15),
                                 claims: claims,
                                 signingCredentials: signingCred
                                );

                            LoggedInUserViewModel loggedInUser = new LoggedInUserViewModel
                            {
                                Id = registeredUser.Id,
                                Email = user.Email,
                                FullName = user.FullName,
                                PhoneNumber = user.PhoneNumber,
                                CompanyId = user.CompanyId,
                                CompanyName = company.CompanyName,
                                TokenString = new JwtSecurityTokenHandler().WriteToken(tokenString),
                                Succeeded = true
                            };
                            return Ok(loggedInUser);
                        }
                        else
                        {
                            return BadRequest(new { result.Errors.FirstOrDefault().Description });
                        }

                    }
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }

            }

            // If we got this far, something failed, redisplay form
            return BadRequest(model);
        }

        [Authorize(Policy = "View Users")]
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserModel>>> GetUsers()
        {
            try
            {
                //Get logged in user
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                //Get logged in user claims
                IList<Claim> loggedInUserClaims = await userManager.GetClaimsAsync(user);
                //Get if logged in user can manage rights
                bool canManageRights = loggedInUserClaims
                    .Any(e => e.Type == "Manage Claims" && e.Value == "true");
                //Get all users in the logged in user's company
                List<ApplicationUser> applicationUsers = await userManager.Users
                    .Where(e => e.CompanyId == user.CompanyId)
                    .ToListAsync();
                //Define return value
                List<UserModel> users = new List<UserModel>();
                foreach (var appUser in applicationUsers)
                {
                    //Define and intialize user claims, avoids exception if user
                    // cannot manage rights
                    List<Claim> appUserClaims = new List<Claim>();
                    if (canManageRights)
                    {
                        //Get user claims if logged in user can manage rights
                        appUserClaims.AddRange(await userManager.GetClaimsAsync(appUser));
                    }
                    users.Add(new UserModel
                    {
                        Email = appUser.Email,
                        FullName = appUser.FullName,
                        PhoneNumber = appUser.PhoneNumber,
                        UserRights = appUserClaims.GetUserRights()
                    });
                }
                return users;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Policy = "Create Users")]
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserModel>> CreateUser(CreateUserModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            var user = new ApplicationUser
            {
                Email = model.Email,
                FullName = model.FullName,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                CompanyId = currentUser.CompanyId
            };
            try
            {
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return StatusCode(500, result.Errors);

                var registeredUser = await userManager.FindByNameAsync(model.Email);

                // Generate validation code
                string code = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);

                result = await userManager.ConfirmEmailAsync(registeredUser, code);

                List<Claim> claims = UserHelper.GetClaims(currentUser.UserType == UserType.Admin);

                await userManager.AddClaimsAsync(registeredUser, claims);

                if (result.Succeeded)
                {
                    return Ok(new UserModel
                    {
                        Email = registeredUser.Email,
                        FullName = registeredUser.FullName,
                        PhoneNumber = registeredUser.PhoneNumber,
                        UserRights = claims.GetUserRights()
                    });
                }
                else
                {
                    return BadRequest(new { error = result.Errors.FirstOrDefault().Description });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [Authorize(Policy = "Edit Users")]
        [HttpPut("EditUser")]
        public async Task<ActionResult<UserModel>> EditUser(UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                return BadRequest(new { error = "Full name requires a value" });
            }
            ApplicationUser user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return model;
                }
                else
                {
                    return StatusCode(500, new { error = "Unable to update user record, please try again later" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Policy = "Manage Claims")]
        [HttpPost("SaveRights")]
        public async Task<ActionResult<UserModel>> SaveRights(UserClaimModel userClaimModel)
        {
            if (string.IsNullOrWhiteSpace(userClaimModel.Username))
            {
                return BadRequest(new { error = "Invalid user" });
            }
            ApplicationUser user = await userManager.FindByNameAsync(userClaimModel.Username);
            if (user == null) return NotFound();

            if (userClaimModel.Claims == null) return BadRequest();
            if (userClaimModel.Claims.Count == 0) return BadRequest();

            IList<Claim> claims = await userManager.GetClaimsAsync(user);

            var result = await userManager.RemoveClaimsAsync(user, claims);
            if (result.Succeeded)
            {
                List<Claim> userClaims = new List<Claim>();
                foreach (var item in userClaimModel.Claims)
                {
                    userClaims.Add(new Claim(item.Type, item.Value));
                }

                result = await userManager.AddClaimsAsync(user, userClaims);
                if (result.Succeeded)
                {
                    UserModel userModel = new UserModel
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        UserRights = userClaims.GetUserRights()
                    };

                    return userModel;
                }
            }

            return BadRequest(new { error = "Error occurred while saving user rights" });

        }


    }
}