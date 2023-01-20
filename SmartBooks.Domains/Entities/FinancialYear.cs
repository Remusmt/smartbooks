using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBooks.Domains.Entities
{
    public class FinancialYear : AppBaseEntity
    {
        public string YearTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? OpeningJournalId { get; set; }
        public int? ClosingJournalId { get; set; }
        public bool Closed { get; set; }

        public Transaction OpeningJournal { get; set; }
        public Transaction ClosingJournal { get; set; }
    }
}
