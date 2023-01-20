using SmartBooks.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Services
{
    public class InvoiceService
    {
        public InvoiceService()
        {

        }

        //public async Task<Invoice> AddAsync(Invoice invoice)
        //{
        //    if (invoice == null) throw new Exception("Invalid invoice");
        //    if (invoice.TransactionItems.Count == 0)
        //        throw new Exception("Invoice must have atleast one item");
        //    if (invoice.OrganisationId == 0)
        //        throw new Exception("Invoice must have a customer selected");

        //    //get reference number if non is set
        //    //Set bill to and ship to address if non is set

        //    return invoice;
        //}
    }
}
