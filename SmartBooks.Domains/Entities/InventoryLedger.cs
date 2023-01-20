using SmartBooks.Domains.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class InventoryLedger : AppBaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public int BinCardId { get; set; }
        public int UnitofMeasureId { get; set; }
        public InventoryTransactionType TransactionType { get; set; }
        /// <summary>
        /// Cost brought forward
        /// </summary>
        public decimal CostBF { get; set; }
        /// <summary>
        /// The cost used to post to general ledger
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// The actual cost the item was received by
        /// </summary>
        public decimal NewCost { get; set; }
        public decimal BalanceBF { get; set; }
        public decimal Balance { get; set; }
        public decimal Quantity { get; set; }
        [StringLength(50)]
        public string SourceDocumentReference { get; set; }
        public byte SourceDocumentType { get; set; }
        public int SourceDocumentId { get; set; }

        public BinCard BinCard { get; set; }
        public UnitofMeasure UnitofMeasure { get; set; }
    }
}
