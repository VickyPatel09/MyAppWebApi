using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Model
{

    public class OrderModel
    {

    }

    public class BuyOrderModel
    {
        public string Name { get; set; }

        public int? Coin { get; set; }

        public int?  Rate { get; set; }
    }

    public class SaleOrderModel
    {
        public string Name { get; set; }

        public int? Coin { get; set; }

        public int? Rate { get; set; }
    }
}
