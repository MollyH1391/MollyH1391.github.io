using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Coupon
    {
        public Coupon()
        {
            Advertises = new HashSet<Advertise>();
            CouponContainers = new HashSet<CouponContainer>();
            CouponProducts = new HashSet<CouponProduct>();
        }

        public int CouponId { get; set; }
        public int ShopId { get; set; }
        public string CouponName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int ThresholdType { get; set; }
        public decimal ThresholdAmount { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Code { get; set; }
        public int? MaxAmount { get; set; }
        public int? Status { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Advertise> Advertises { get; set; }
        public virtual ICollection<CouponContainer> CouponContainers { get; set; }
        public virtual ICollection<CouponProduct> CouponProducts { get; set; }
    }
}
