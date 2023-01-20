using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class JournalSpecification : BaseSpecification<Journal>
    {
        public JournalSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted)
        {
            AddInclude(e => e.JournalDetails);
        }

    }
}
