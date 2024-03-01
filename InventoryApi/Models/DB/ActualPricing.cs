using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class ActualPricing
    {
        public int PricingId { get; set; }
        public int ItemId { get; set; }
        public DateTime LastUpdate { get; set; }
        public decimal Price { get; set; }

        public virtual Item Item { get; set; } = null!;
    }
}
