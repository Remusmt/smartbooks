using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    /// <summary>
    /// Used to define payment terms used on invoices and bills
    /// </summary>
    public class PaymentTerm : AppBaseEntity
    {
        /// <summary>
        /// Name describing the payment terms
        /// </summary>
        [StringLength(150)]
        public string Description { get; set; }
        /// <summary>
        /// Set true if the terms are based on dates, otherwise driven by number of days
        /// </summary>
        public bool DateDriven { get; set; }
        /// <summary>
        /// Use to set numberof days, to when net payment is due
        /// </summary>
        public int NetDueIn { get; set; }
        /// <summary>
        /// Use to set the percentage discount, if any
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// Use to set number of days within which a discount is given, if payment is made
        /// </summary>
        public int DiscountIfPaidWithin { get; set; }
        /// <summary>
        /// Use to set the day of the month when net due for date based terms
        /// </summary>
        public int NetDueBefore { get; set; }
        /// <summary>
        /// Make due date next month if date is past this day of the month
        /// </summary>
        public int DueNextMonthIfIssued { get; set; }
        /// <summary>
        /// Give the discount percentage if paid before this day of month
        /// </summary>
        public int DiscountIfPaidBefore { get; set; }
    }
}
