using SmartBooks.Domains.Entities;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class InventoryItemsListModel
    {
        public InventoryItemsListModel()
        {
            InventoryItems = new List<InventoryItem>();
        }
        public List<InventoryItem> InventoryItems { get; set; }
        public int TotalCount { get; set; }
    }
}
