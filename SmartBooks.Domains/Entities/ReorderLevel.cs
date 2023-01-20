namespace SmartBooks.Domains.Entities
{
    public class ReorderLevel : AppBaseEntity
    {
        public int InventoryItemId { get; set; }
        public int UnitofMeasureId { get; set; }
        public bool GlobalBased { get; set; }
        public int? WarehouseId { get; set; }
        public int? BinId { get; set; }
        public decimal SafetyStock { get; set; }
        public decimal ReorderPoint { get; set; }

        public InventoryItem InventoryItem { get; set; }
        public UnitofMeasure UnitofMeasure { get; set; }
        public Warehouse Warehouse { get; set; }
        public Bin Bin { get; set; }
    }
}
