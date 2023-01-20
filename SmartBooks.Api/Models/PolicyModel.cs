using System.Collections.Generic;

namespace SmartBooks.Api.Models
{
    public class UserModel
    {
        public UserModel()
        {
            UserRights = new List<ClaimsGroupModel>();
        }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<ClaimsGroupModel> UserRights { get; set; }
    }
    public class ClaimsGroupModel
    {
        public ClaimsGroupModel()
        {
            Rights = new List<ClaimModel>();
        }
        public string Description { get; set; }
        public List<ClaimModel> Rights { get; set; }
    }

    public class ClaimModel
    {
        public string Description { get; set; }
        public string ClaimType { get; set; }
        public bool Granted { get; set; }
    }
}
