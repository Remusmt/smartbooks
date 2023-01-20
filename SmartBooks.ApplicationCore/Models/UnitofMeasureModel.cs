using SmartBooks.Domains.Enums;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class UnitofMeasureListModel
    {
        public UnitofMeasureListModel()
        {
            UomConversions = new HashSet<UomConversionModel>();
        }
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public UomType Type { get; set; }
        public ICollection<UomConversionModel> UomConversions { get; set; }
    }

    public class UomConversionModel
    {
        public int Id { get; set; }
        public int UnitofMeasureFromId { get; set; }
        public int UnitofMeasureToId { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
    }
}
