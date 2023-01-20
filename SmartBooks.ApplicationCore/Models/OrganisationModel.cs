using SmartBooks.Domains.Entities;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class OrganisationModel<T>
        where T: Organisation
    {
        public OrganisationModel()
        {
            Organisations = new List<T>();
        }
        public List<T> Organisations { get; set; }
        public int TotalCount { get; set; }
    }
}
