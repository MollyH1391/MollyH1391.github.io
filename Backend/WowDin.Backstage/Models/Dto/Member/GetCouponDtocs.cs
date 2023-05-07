using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Member
{
    public class GetCouponDto
    {
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<string> Status { get; set; }
        public IEnumerable<CouponDto> CouponsForDiscount { get; set; }
        public IEnumerable<CouponDto> CouponsForVoucher { get; set; }
    }

    public class CouponDto
    {
        public string ShopName { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeSpan { get; set; }
        public string RemainDays { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }
}
