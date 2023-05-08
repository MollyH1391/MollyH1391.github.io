using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class ShopMethod
    {
        public ShopMethod()
        {
            Deliveries = new HashSet<Delivery>();
            Takeouts = new HashSet<Takeout>();
        }

        public int ShopMethodId { get; set; }
        public int ShopId { get; set; }
        public int TakeMethod { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Takeout> Takeouts { get; set; }
    }
}
