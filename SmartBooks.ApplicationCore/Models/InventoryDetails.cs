using SmartBooks.Domains.Enums;
using System;

namespace SmartBooks.ApplicationCore.Models
{
    public class InventoryDetails
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime TransactionDate { get; set; }
        public int InventoryItemId { get; set; }
        public int UnitofMeasureId { get; set; }
        public int BinId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OtherCosts { get; set; }
        public string SourceDocumentReference { get; set; }
        public byte SourceDocumentType { get; set; }
        public int SourceDocumentId { get; set; }
        public InventoryTransactionType InventoryTransaction { get; set; }
        public bool AllowNegativeIssue { get; set; }
    }
}
