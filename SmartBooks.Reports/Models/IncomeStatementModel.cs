using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBooks.Reports.Models
{
    public class IncomeStatementModel
    {
        public IncomeStatementModel()
        {
            IncomeAccountItems = new List<AccountModel>();
            COGSAccountItems = new List<AccountModel>();
            OperatingExpenseAccountItems = new List<AccountModel>();
            OtherIncomeandExpensesAccountItems = new List<AccountModel>();
        }
        /// <summary>
        /// This can be a month a quarter or a year, the assumption is we pick all transactions between two specific dates
        /// </summary>
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        /// <summary>
        /// List of all primary income accounts
        /// </summary>
        public List<AccountModel> IncomeAccountItems { get; set; }
        /// <summary>
        /// Total of all the income accounts
        /// </summary>
        public decimal TotalSales
        {
            get
            {
                return IncomeAccountItems.Sum(e => e.PeriodAmount);
            }
        }
        /// <summary>
        /// List of all COGS accounts
        /// </summary>
        public List<AccountModel> COGSAccountItems { get; set; }
        /// <summary>
        /// Total of all cost of goods accounts
        /// </summary>
        public decimal TotalCOGS
        {
            get
            {
                return COGSAccountItems.Sum(e => e.PeriodAmount);
            }
        }
        /// <summary>
        /// Differences between {{TotalSales}} - {{TotalCOGS}}
        /// </summary>
        public decimal GrossProfit
        {
            get
            {
                return TotalSales - TotalCOGS;
            }
        }
        public decimal GrossMargin
        {
            get
            {
                try
                {
                    return GrossProfit / TotalSales * 100;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        /// <summary>
        /// List of all operating expense accounts
        /// </summary>
        public List<AccountModel> OperatingExpenseAccountItems { get; set; }
        /// <summary>
        /// All operating expense accounts sum
        /// </summary>
        public decimal TotalOperatingExpense
        {
            get
            {
                return OperatingExpenseAccountItems.Sum(e => e.PeriodAmount);
            }
        }
        /// <summary>
        /// Differences between {{GrossProfit}} - {{TotalOperatingExpense}}
        /// </summary>
        public decimal OperatingProfit
        {
            get
            {
                return GrossProfit - TotalOperatingExpense;
            }
        }
        public decimal OperatingMargin
        {
            get
            {
                try
                {
                    return OperatingProfit / TotalSales * 100;
                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }
        public List<AccountModel> OtherIncomeandExpensesAccountItems { get; set; }
        public decimal TotalOtherIncomeAndExpenses
        {
            get
            {
                return OtherIncomeandExpensesAccountItems.Sum(e => e.PeriodAmount);
            }
        }
        /// <summary>
        /// Differences between {{OperatingProfit}} - {{TotalOtherIncomeAndExpenses}}
        /// </summary>
        public decimal PreTaxIncome
        {
            get
            {
                return OperatingProfit - TotalOtherIncomeAndExpenses;
            }
        }

        public decimal PretaxMargin
        {
            get
            {
                try
                {
                    return PreTaxIncome / TotalSales * 100;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public decimal IncomeTax { get; set; }

        public decimal NetIncome
        {
            get
            {
                return PreTaxIncome - IncomeTax;
            }
        }
        public decimal NetIncomeMargin
        {
            get
            {
                try
                {
                    return NetIncome / TotalSales * 100;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }



    }

}
