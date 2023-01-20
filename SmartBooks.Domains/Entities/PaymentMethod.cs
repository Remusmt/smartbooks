using SmartBooks.Domains.Enums;

namespace SmartBooks.Domains.Entities
{
    public class PaymentMethod : AppBaseEntity
    {
        public PaymentMethodType PaymentMethodType { get; set; }
        public string Description { get; set; }
        public int LedgerAccountId { get; set; }

        public LedgerAccount LedgerAccount { get; set; }
    }
}
