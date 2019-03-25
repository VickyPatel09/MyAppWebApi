using System;
using System.Collections.Generic;

namespace MyAngularAppCore.Entities
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? TransactionType { get; set; }
        public int? BuyUserId { get; set; }
        public int? SaleUserId { get; set; }
        public int? Coin { get; set; }
        public int? Rate { get; set; }
        public DateTime? TransactionDateTime { get; set; }

        public Users BuyUser { get; set; }
        public Users SaleUser { get; set; }
        public Users User { get; set; }
    }
}
