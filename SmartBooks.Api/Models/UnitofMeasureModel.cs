using SmartBooks.Domains.Enums;

namespace SmartBooks.Api.Models
{
    public class UnitofMeasureModel
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public UomType Type { get; set; }
    }
}
