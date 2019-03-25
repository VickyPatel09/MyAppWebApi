using System;
using System.Collections.Generic;

namespace MyAngularAppCore.Entities
{
    public partial class Users
    {
        public Users()
        {
            BuyOrder = new HashSet<BuyOrder>();
            SaleOrder = new HashSet<SaleOrder>();
            TransactionBuyUser = new HashSet<Transaction>();
            TransactionSaleUser = new HashSet<Transaction>();
            TransactionUser = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? WalletAmount { get; set; }
        public string S { get; set; }
        public int? Status { get; set; }

        public ICollection<BuyOrder> BuyOrder { get; set; }
        public ICollection<SaleOrder> SaleOrder { get; set; }
        public ICollection<Transaction> TransactionBuyUser { get; set; }
        public ICollection<Transaction> TransactionSaleUser { get; set; }
        public ICollection<Transaction> TransactionUser { get; set; }
    }
}
