using System.Collections.Generic;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// This holds the record of an inventory item on a specifc bin
    /// </summary>
    public class BinCard : AppBaseEntity
    {
        public BinCard()
        {
            InventoryLedgers = new HashSet<InventoryLedger>();
        }
        public int BinId { get; set; }
        public int InventoryItemId { get; set; }
        public decimal Balance { get; set; }

        public Bin Bin { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public ICollection<InventoryLedger> InventoryLedgers { get; set; }
    }
}
