using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Coupon
{
    public class CreateCouponDto
    {
        public ShopAndProductDto CouponBelong { get; set; }
        public string CouponName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Type { get; set; }
        public decimal ThresholdAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int? MaxAmount { get; set; }
        public string Description { get; set; }
        //public string Code { get; set; }
        public int Status { get; set; }
    }

    public class ShopAndProductDto
    {
        public int ShopId { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
    }
}
