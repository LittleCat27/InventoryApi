using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Usuario
    {
        public short UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Mail { get; set; } = null!;
    }
}
