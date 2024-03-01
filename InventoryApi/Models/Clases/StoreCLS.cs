namespace InventoryApi.Models.Clases
{
    public class StoreCLS
    {
        public int StoreId { get; set; }
        public string Name { get; set; } = null!;
        public int AddressId { get; set; }

        public string AddressName { get; set; }
    }
}
