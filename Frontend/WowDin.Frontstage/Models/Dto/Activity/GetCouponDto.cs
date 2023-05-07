using System;
using System.Collections.Generic;
using WowDin.Frontstage.Models.Entity;

namespace WowDin.Frontstage.Models.Dto
{
    public class GetCouponDto
    {
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<string> Status { get; set; }
        public IEnumerable<CouponDto> CouponsForDiscount { get; set; }
        public IEnumerable<CouponDto> CouponsForVoucher { get; set; }
        public IEnumerable<CouponDto> CouponsForDiscountToast { get; set; }
        public IEnumerable<CouponDto> CouponsForVoucherToast { get; set; }
        public IEnumerable<CouponDto> CouponsForAddVoucher { get; set; }
        public IEnumerable<CouponDto> CouponsForAddDiscount { get; set; }
    }

    public class CouponDto
    {
        public string BrandLogo { get; set; }
        public string ShopName { get; set; }
        public int ShopId { get; set; }
        public int CouponId { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeSpan { get; set; }
        public string RemainDays { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }

    public class ShopData
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
    }
}
