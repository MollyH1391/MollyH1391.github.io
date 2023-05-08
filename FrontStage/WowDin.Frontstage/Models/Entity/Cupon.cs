using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class Cupon
    {
        public Cupon()
        {
            Advertises = new HashSet<Advertise>();
            CuponContainers = new HashSet<CuponContainer>();
            CuponProducts = new HashSet<CuponProduct>();
        }

        public int CuponId { get; set; }
        public int ShopId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int ThresholdType { get; set; }
        public decimal ThresholdAmount { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Code { get; set; }
        public int? MaxAmount { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Advertise> Advertises { get; set; }
        public virtual ICollection<CuponContainer> CuponContainers { get; set; }
        public virtual ICollection<CuponProduct> CuponProducts { get; set; }
    }
}
