using System;
using System.Collections.Generic;
using WowDin.Backstage.Models.Entity;

namespace WowDin.Backstage.Models.ViewModel.Coupon
{
    public class CouponViewModel
    {

        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<string> Status { get; set; }
        public IEnumerable<CouponDetail> CouponsForDiscount { get; set; }
        public IEnumerable<CouponDetail> CouponsForVoucher { get; set; }
        public IEnumerable<CouponDetail> CouponsForDiscountToast { get; set; }
        public IEnumerable<CouponDetail> CouponsForVoucherToast { get; set; }
    }

    public class CouponDetail
    {
        public string ShopName { get; set; }
        public int DiscountPrice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeSpan { get; set; }
        public string RestTime { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }
}
