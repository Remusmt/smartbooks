using SmartBooks.Domains.Entities;
using System;

namespace SmartBooks.Domains.SchoolEntities
{
    public class Student : SubLedgerBase
    {
        public DateTime AdmissionDate { get; set; }
        public string PreviousSchool { get; set; }
        public int JoinedLevelId { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string Relationship { get; set; }

        public Level JoinedLevel { get; set; }
    }
}
