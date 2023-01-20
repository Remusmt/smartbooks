using SmartBooks.Domains.Enums;

namespace SmartBooks.Api.Models
{
    public class InventoryItemModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public InventoryItemType Type { get; set; }
        public int UnitofMeasureId { get; set; }
        public int? InventoryCategoryId { get; set; }
        public int? TaxId { get; set; }
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public int? AssetAcount { get; set; }
        public int? CogsAccount { get; set; }
        public int? IncomeAccount { get; set; }
    }
}
