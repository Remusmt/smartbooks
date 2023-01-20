using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class Currency : BaseEntity
    {
        public int CountryId { get; set; }
        [StringLength(50)]
        public string ISOCode { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public string  Symbol { get; set; }
        public int Priority { get; set; }
        public bool SymbolFirst { get; set; }
        public string DecimalMark { get; set; }
        public string ThousandSeparator { get; set; }
        public int NumericCode { get; set; }
        public string SubUnit { get; set; }
        public int SubUnitToUnit { get; set; }

        public Country Country { get; set; }
    }
}
