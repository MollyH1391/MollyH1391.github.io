using System.Collections.Generic;
using WowDin.Backstage.Models.Base;
using WowDin.Backstage.Models.Dto.Coupon;

namespace WowDin.Backstage.Services.Interface
{
    public interface ICouponService
    {
        public AllCouponDto GetAllCouponOfBrand(int brandId);
        public ShopsForCouponDto GetShopsAndProducts(int brandId);
        public APIResult CreateCoupon(CreateCouponDto coupon, string code);
        public APIResult EditCoupon(EditCouponDto coupon);
        public APIResult SwitchCouponStatus(int couponId);
    }
}
