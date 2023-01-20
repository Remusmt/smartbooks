using SmartBooks.Domains.Enums;
using System.Collections.Generic;

namespace SmartBooks.Api.Models
{
    public class MenuGroupModel
    {
        public MenuGroupModel()
        {
            Menus = new List<MenuItem>();
        }
        public string GroupName { get; set; }
        public string SystemModule { get; set; }
        public string Icon { get; set; }
        public bool ImageIcon { get; set; }
        public List<MenuItem> Menus { get; set; }
    }

    public class MenuItem
    {
        public string Description { get; set; }
        public string SystemType { get; set; }
        public string LinkUrl { get; set; }
        public bool ImageIcon { get; set; }
        public string Icon { get; set; }
        public CompanyType[] CompanyTypes { get; set; }
    }
}
