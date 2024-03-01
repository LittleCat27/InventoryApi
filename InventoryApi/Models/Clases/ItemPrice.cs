namespace InventoryApi.Models.Clases
{
    public class ItemPrice
    {
        public int ItemId { get; set; }
        public int PriceId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
