namespace InventoryApi.Models.Clases
{
    public class UsuarioCLS
    {
        public UsuarioCLS()
        {
            this.UserId = 0;
            this.Username = string.Empty;
            this.Mail = string.Empty;
            this.LoginOK = 0;
        }
        public short UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public int LoginOK { get; set; }    
    }
}
