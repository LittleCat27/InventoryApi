using InventoryApi.Models;

namespace InventoryApi.Models.Clases
{
    public class ItemCLS
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Code { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }

        public List<InventoryCLS> inventory { get; set; }

        public List<CategoryCLS> categories { get; set; }

    }
}
