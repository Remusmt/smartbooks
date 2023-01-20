using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using SmartBooks.Api.Models;
using SmartBooks.Domains.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace SmartBooks.Api.Helpers
{
    public static class UserHelper
    {
        public static AuthorizationOptions GetPolicies(this AuthorizationOptions options)
        {
            //options.AddPolicy("Create Warehouse", policy =>
            //    policy.RequireAssertion(context => context.User.HasClaim("Create Warehouse", "true") || context.User.IsInRole("Super User")));
            foreach (var claimGroup in GetClaimsFromFile())
            {
                foreach (ClaimModel claim in claimGroup.Rights)
                {
                    options.AddPolicy(claim.ClaimType, policy => policy.RequireClaim(claim.ClaimType, "true"));
                }
            }

            return options;
        }

        private static List<ClaimsGroupModel> GetClaimsFromFile()
        {
            var claimsPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "Data", "Claims.json" });
            string claimsJsonString = File.ReadAllText(claimsPath);
            return JsonConvert.DeserializeObject<List<ClaimsGroupModel>>(claimsJsonString);
        }

        public static List<Claim> GetClaims(bool IsAdmin)
        {
            List<Claim> claims = new();
            foreach (var claimGroup in GetClaimsFromFile())
            {
                foreach (ClaimModel claim in claimGroup.Rights)
                {
                    claims.Add(new Claim(claim.ClaimType, IsAdmin ? "true" : "false"));
                }
            }

            return claims;
        }

        public static List<MenuModel> GetMenus(
            this List<Claim> claims,
            CompanyType companyType)
        {
            var menuPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "Data", "Menu.json" });
            string menuJsonString = File.ReadAllText(menuPath);
            List<MenuGroupModel> menuGroups = JsonConvert.DeserializeObject<List<MenuGroupModel>>(menuJsonString);

            List<MenuModel> menus = new();

            foreach (var menuGroup in menuGroups)
            {
                MenuModel menu = new()
                {
                    Description = menuGroup.GroupName,
                    Icon = menuGroup.Icon,
                    ImageIcon = menuGroup.ImageIcon
                };
                foreach (var menuItem in menuGroup.Menus)
                {
                    if (claims.Any(e => e.Type == $"View {menuItem.SystemType}" && e.Value == "true"))
                    {
                        menu.Links.Add(
                            new MenuModel
                            {
                                Description = menuItem.Description,
                                LinkUrl = menuItem.LinkUrl,
                                Icon = menuItem.Icon,
                                ImageIcon = menuItem.ImageIcon
                            });
                    }
                }
                if (menu.Links.Count > 0)
                {
                    menus.Add(menu);
                }
            }

            return menus;
        }

        public static List<ClaimsGroupModel> GetUserRights(this List<Claim> claims)
        {
            List<ClaimsGroupModel> claimsGroups = new List<ClaimsGroupModel>();
            //if user has no claims return empty list
            if (claims.Count == 0) return claimsGroups;

            foreach (var claimGroup in GetClaimsFromFile())
            {
                ClaimsGroupModel groupModel = new ClaimsGroupModel
                {
                    Description = claimGroup.Description
                };
                foreach (ClaimModel claim in claimGroup.Rights)
                {
                    Claim right = claims.FirstOrDefault(e => e.Type == claim.ClaimType);
                    groupModel.Rights.Add(new ClaimModel
                    {
                        ClaimType = claim.ClaimType,
                        Description = claim.Description,
                        Granted = right != null && right.Value == "true"
                    });
                }
                claimsGroups.Add(groupModel);
            }
            return claimsGroups;
        }
    }
}
