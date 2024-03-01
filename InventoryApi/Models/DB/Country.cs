using System;
using System.Collections.Generic;

namespace InventoryApi
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public short CountryId { get; set; }
        public string Country1 { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
