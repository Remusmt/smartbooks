using SmartBooks.Domains.Entities;
using System;
using System.Collections.Generic;

namespace SmartBooks.Domains.SchoolEntities
{
    public class SchoolYear : AppBaseEntity
    {
        public SchoolYear()
        {
            SchoolTerms = new HashSet<SchoolTerm>();
        }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        public ICollection<SchoolTerm> SchoolTerms { get; set; }
    }
}
