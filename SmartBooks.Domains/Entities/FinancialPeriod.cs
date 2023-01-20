using System;

namespace SmartBooks.Domains.Entities
{
    public class FinancialPeriod : AppBaseEntity
    {
        public FinancialPeriod()
        {
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public int Month
        {
            get
            {
                return StartDate.Month;
            }
        }
        public string Description
        {
            get
            {
                return Month switch
                {
                    1 => "January",
                    2 => "February",
                    3 => "March",
                    4 => "April",
                    5 => "May",
                    6 => "June",
                    7 => "July",
                    8 => "August",
                    9 => "September",
                    10 => "October",
                    11 => "November",
                    12 => "December",
                    _ => "",
                };
            }
        }
        public string ShortDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Description)) return "";
                return Description.Substring(0, 3);
            }
        }
        public bool IsOpen { get; set; }
        public bool IsMoved { get; set; }
        public bool IsClosed { get; set; }

    }
}
