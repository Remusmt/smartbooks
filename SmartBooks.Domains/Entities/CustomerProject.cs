using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// This class can be used to track or group customer transactions
    /// it should be a field on invoices, cash sales, credit notes or 
    /// any  document with a customer reference
    /// </summary>
    public class CustomerProject : AppBaseEntity
    {
        public int CustomerId { get; set; }
        [StringLength(150)]
        public string Description { get; set; }

        public Customer Customer { get; set; }
    }
}
