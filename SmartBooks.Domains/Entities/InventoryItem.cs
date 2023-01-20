using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.Entities
{
    public class InventoryItem : TransactionItem //Create item that this and sacco member accounts can inherit from
    {
        public InventoryItemType Type { get; set; }
        public int UnitofMeasureId { get; set; }
        public int? InventoryCategoryId { get; set; }
        public int? TaxId { get; set; }
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public decimal AverageCost { get; set; }
        public decimal LastCost { get; set; }
        /// <summary>
        /// Total items in stock
        /// </summary>
        public decimal OnHand { get; set; }
        public decimal OnOrder { get; set; }
        public decimal Allocated { get; set; }
        public decimal BackOrdered { get; set; }
        public decimal Available
        {
            get
            {
                return OnHand - Allocated;
            }
        }
        public int? AssetAcount { get; set; }
        public int? CogsAccount { get; set; }
        public int? IncomeAccount { get; set; }
        public decimal TotalAverageCost
        {
            get
            {
                return AverageCost * OnHand;
            }
        }
        public decimal TotalStandardCost
        {
            get
            {
                return StandardCost * OnHand;
            }
        }

        public UnitofMeasure UnitofMeasure { get; set; }
        public InventoryCategory InventoryCategory { get; set; }
    }
}
