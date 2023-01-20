using SmartBooks.Domains.Entities;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class SupplierListModel
    {
        public SupplierListModel()
        {
            Organisations = new List<Supplier>();
        }
        public List<Supplier> Organisations { get; set; }
        public int TotalCount { get; set; }
    }
}
