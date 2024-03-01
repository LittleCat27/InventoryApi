namespace InventoryApi.Models.Clases
{
    public class AddressCLS
    {
        public int id { get; set; }

        public string name { get; set; } = null!;
        public int cityId { get; set; }
        public string? postalCode { get; set; }
    }
}
