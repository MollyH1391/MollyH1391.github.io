using System.Collections.Generic;
using WowDin.Frontstage.Models.Dto;
using WowDin.Frontstage.Models.Dto.Activity;
using WowDin.Frontstage.Models.Dto.Home;

namespace WowDin.Frontstage.Services
{
    public interface IActivityService
    {
        GetCouponDto GetCouponByUserId(int id);
        GetIndexFiguresDto GetIndexFigures();
        public IEnumerable<UsableCouponByUserDto> GetUsableCouponByUser(int userId, int shopId);
        public CalculateCouponDiscountDto CalculateCouponDiscount(int cartId, int couponId);
        public CouponResultDto RequestForCoupon(string code, int userId);
        public CouponContentDto GetCoupon(string code, int userId);

    }
}