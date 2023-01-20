using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBooks.Domains.Entities
{
    public class Invoice : Transaction
    {
        public Invoice()
        {
            DocumentName = "Invoice";
            DocumentType = Enums.TransactionType.Invoice;
        }

        public DateTime DueDate { get; set; }

        public decimal CostofGoods { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }

        public decimal WithheldAmount { get; set; }
        public decimal ReturnedCreditAmount { get; set; }
        public decimal AddedDebitAmount { get; set; }
    }

}
