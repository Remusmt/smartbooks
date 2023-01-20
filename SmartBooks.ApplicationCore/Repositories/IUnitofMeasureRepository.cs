using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IUnitofMeasureRepository<T> : IRepository<T>
        where T : UnitofMeasure
    {
        //Abbreviation
        bool AbbreviationExists(string abbreviation, int companyId);
        bool DuplicateAbbreviation(int id, string abbreviation, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
    }
}
