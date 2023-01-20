using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Api.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoggedInUserViewModel
    {
        public LoggedInUserViewModel()
        {
            Menus = new List<MenuModel>();
            CompanyDefault = new CompanyDefault();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string TokenString { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Succeeded { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool CanManageUsers { get; set; }
        public CompanyDefault CompanyDefault { get; set; }
        public List<MenuModel> Menus { get; set; }
    }

    public class MenuModel
    {
        public MenuModel()
        {
            Links = new List<MenuModel>(); 
        }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string Icon { get; set; }
        public bool ImageIcon { get; set; }
        public List<MenuModel> Links { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public int CountryId { get; set; }
    }

    public class CreateUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserClaimModel
    {
        public string Username { get; set; }
        public List<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class CompanyDefault
    {
        public int DefaultCurrency { get; set; }
        public bool UseAccountNumbers { get; set; }
        public bool AllowPostingToParentAccount { get; set; }
        public bool EnableInventoryTracking { get; set; }
        public int DefaultWarehouse { get; set; }
        public bool HasMultipleWarehouses { get; set; }
    }
}
