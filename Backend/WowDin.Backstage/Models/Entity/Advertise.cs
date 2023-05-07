using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Advertise
    {
        public int AdvertiseId { get; set; }
        public string Img { get; set; }
        public bool IsSearchZone { get; set; }
        public int? CouponId { get; set; }
        public int? Sort { get; set; }
        public int BrandId { get; set; }
        public int? Status { get; set; }
        public string Message { get; set; }

        public virtual Coupon Coupon { get; set; }
    }
}
