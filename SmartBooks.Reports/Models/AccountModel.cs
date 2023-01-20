using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBooks.Reports.Models
{
    public class AccountModel
    {
        public MainAccountType MainAccountType { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CurrencyBalance { get; set; }

        public decimal BalanceBF { get; set; }
        public decimal PeriodAmount { get; set; }
    }
}
