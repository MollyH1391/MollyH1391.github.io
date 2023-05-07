using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class CouponContainer
    {
        public int CouponContainerId { get; set; }
        public int UserAccountId { get; set; }
        public int CouponId { get; set; }
        public int CouponState { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
