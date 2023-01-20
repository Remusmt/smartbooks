using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface ISchoolBaseRepository<T> : IRepository<T>
        where T : SchoolBaseClass
    {
        bool DescriptionExists(string description, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
    }
}
