using SmartBooks.Domains.Entities;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class UomConversionSpecification : BaseSpecification<UomConversion>
    {
        public UomConversionSpecification(int uomId)
            : base(e => (e.UnitofMeasureFromId == uomId || e.UnitofMeasureToId == uomId)
            && !e.IsDeleted)
        {
        }
    }
}
