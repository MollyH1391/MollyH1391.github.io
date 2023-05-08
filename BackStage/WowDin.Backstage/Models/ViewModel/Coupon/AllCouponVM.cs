using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Coupon
{
    public class AllCouponVM
    {
        IEnumerable<CouponDetailVM> Coupons { get; set; }
    }

    public class CouponDetailVM
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Products { get; set; }
        public string CouponName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Type { get; set; }
        public decimal ThresholdAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int? MaxAmount { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int ReleasedAmount { get; set; }
        public string Status { get; set; }
    }
}
