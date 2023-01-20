using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// This represents a specific location in a warehouses
    /// </summary>
    public class Bin : AppBaseEntity
    {
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
