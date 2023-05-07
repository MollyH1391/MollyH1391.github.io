using System;
using System.Collections.Generic;
using WowDin.Frontstage.Models.Entity;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class CouponViewModel
    {
        public List<string> Brands { get; set; }
        public List<string> Status { get; set; }
        public IEnumerable<CouponDetail> CouponsForDiscount { get; set; }
        public IEnumerable<CouponDetail> CouponsForVoucher { get; set; }

    }

    public class CouponDetail
    {
        public string BrandLogo { get; set; }
        public string ShopName { get; set; }
        public int ShopId { get; set; }
        public int CouponId { get; set; }
        public int DiscountPrice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeSpan { get; set; }
        public string RestTime { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }

}
