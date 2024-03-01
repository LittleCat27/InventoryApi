using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
    }
}
