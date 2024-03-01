using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Store
    {
        public Store()
        {
            Inventories = new HashSet<Inventory>();
        }

        public int StoreId { get; set; }
        public string Name { get; set; } = null!;
        public int AddressId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
