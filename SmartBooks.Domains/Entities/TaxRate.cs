using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class TaxRate : AppBaseEntity
    {
        public int TaxId { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public decimal SalesRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public bool PurchaseTaxIsReclaimable { get; set; }

        public Tax Tax { get; set; }

        public string QualifiedDescription
        {
            get
            {
                return $"{Tax?.Description}: {Description}";
            }
        }
    }
}
