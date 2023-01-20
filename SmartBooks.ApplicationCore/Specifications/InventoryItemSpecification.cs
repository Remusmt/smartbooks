using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;

namespace SmartBooks.ApplicationCore.Specifications
{
    public class InventoryItemSpecification : BaseSpecification<InventoryItem>
    {
        public InventoryItemSpecification(int companyId)
            : base(e => e.CompanyId == companyId && !e.IsDeleted 
            && e.Type != InventoryItemType.SystemItem) 
        {
            ApplyOrderBy(e => e.Description);
        }

        public InventoryItemSpecification(int companyId, string sort, string order, int page, int pageSize)
            : base(e => e.CompanyId == companyId && !e.IsDeleted
            && e.Type != InventoryItemType.SystemItem)
        {
            AddInclude("UnitofMeasure");
            ApplyPaging((page - 1) * pageSize, pageSize);
            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (order == "desc")
                {
                    switch (sort.ToLower())
                    {
                        case "description" :
                            ApplyOrderByDescending(e => e.Description);
                            break;
                        case "code":
                            ApplyOrderByDescending(e => e.Code);
                            break;
                        case "onhand":
                            ApplyOrderByDescending(e => e.OnHand);
                            break;
                    }
                }
                else
                {
                    switch (sort.ToLower())
                    {
                        case "description":
                            ApplyOrderBy(e => e.Description);
                            break;
                        case "code":
                            ApplyOrderBy(e => e.Code);
                            break;
                        case "onhand":
                            ApplyOrderBy(e => e.OnHand);
                            break;
                    }
                }
            }
        }

    }
}
