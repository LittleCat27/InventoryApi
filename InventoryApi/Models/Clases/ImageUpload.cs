namespace InventoryApi.Models.Clases
{
    public class ImageUpload
    {
        public int ItemId { get; set; }
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }
    }
}
