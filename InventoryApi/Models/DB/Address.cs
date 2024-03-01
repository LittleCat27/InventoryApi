using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Address
    {
        public Address()
        {
            Stores = new HashSet<Store>();
        }

        public int AddressId { get; set; }
        public string Address1 { get; set; } = null!;
        public int CityId { get; set; }
        public string? PostalCode { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Store> Stores { get; set; }
    }
}
