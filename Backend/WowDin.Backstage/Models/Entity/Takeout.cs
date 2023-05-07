using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Takeout
    {
        public int TakeoutId { get; set; }
        public int ShopMethodId { get; set; }
        public TimeSpan WaitingTime { get; set; }
        public string Condition { get; set; }

        public virtual ShopMethod ShopMethod { get; set; }
    }
}
