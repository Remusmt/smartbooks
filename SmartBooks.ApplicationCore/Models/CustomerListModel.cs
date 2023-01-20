using SmartBooks.Domains.Entities;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class CustomerListModel
    {
        public CustomerListModel()
        {
            Customers = new List<Customer>();
        }
        public List<Customer> Customers { get; set; }
        public int TotalCount { get; set; }
    }
}
