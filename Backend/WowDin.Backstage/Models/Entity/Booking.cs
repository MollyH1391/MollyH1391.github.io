using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Booking
    {
        public int ShopMethodId { get; set; }
        public string 待討論 { get; set; }

        public virtual ShopMethod ShopMethod { get; set; }
    }
}
