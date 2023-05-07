using System;

namespace WowDin.Backstage.Models.Dto.Coupon
{
    public class EditCouponVM
    {
        public int Id { get; set; }
        public string CouponName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? MaxAmount { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
