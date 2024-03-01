using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class City
    {
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        public int CityId { get; set; }
        public string City1 { get; set; } = null!;
        public short CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
