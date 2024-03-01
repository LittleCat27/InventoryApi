using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Item
    {
        public Item()
        {
            ActualPricings = new HashSet<ActualPricing>();
            Inventories = new HashSet<Inventory>();
            Categories = new HashSet<Category>();
        }

        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Code { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public virtual ICollection<ActualPricing> ActualPricings { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
