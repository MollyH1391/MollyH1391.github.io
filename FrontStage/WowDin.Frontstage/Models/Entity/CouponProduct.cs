using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CouponProduct
    {
        public int CouponId { get; set; }
        public int ProductId { get; set; }
        public int CouponProductId { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual Product Product { get; set; }
    }
}
