﻿using System.ComponentModel.DataAnnotations;

namespace SmartBooks.Domains.Entities
{
    public class TransactionItem : AppBaseEntity
    {
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
    }
}
