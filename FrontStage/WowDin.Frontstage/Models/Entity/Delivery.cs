using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class Delivery
    {
        public int DeliveryId { get; set; }
        public int ShopMethodId { get; set; }
        public decimal? PriceThreshold { get; set; }
        public double LowerDistance { get; set; }
        public double? HigherDistance { get; set; }
        public decimal? DeliveryFee { get; set; }
        public TimeSpan? WaitingTime { get; set; }

        public virtual ShopMethod ShopMethod { get; set; }
    }
}
