using System;
using System.Collections.Generic;

namespace MyAngularAppCore.Entities
{
    public partial class BuyOrder
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public int? Coin { get; set; }
        public int? Rate { get; set; }

        public Users User { get; set; }
    }
}
