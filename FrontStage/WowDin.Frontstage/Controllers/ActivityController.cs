using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WowDin.Frontstage.Models.ViewModel.Member;
using WowDin.Frontstage.Services;

namespace WowDin.Frontstage.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityservice;
        private readonly IMapper _mapper;


        public ActivityController(IActivityService activityservice, IMapper mapper)
        {
            _activityservice = activityservice;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [Route("/Activity/Coupon/{code?}")]
        public IActionResult Coupon(string code)
        {
            var id = int.Parse(User.Identity.Name);
            var couponDto = _activityservice.GetCouponByUserId(id);
            var result = new CouponViewModel()
            {
                Brands = couponDto.Brands.ToList(),
                Status = couponDto.Status.ToList(),
                CouponsForDiscount = couponDto.CouponsForDiscount.Select(x => new CouponDetail
                {
                    BrandLogo = x.BrandLogo,
                    ShopName = x.ShopName,
                    ShopId = x.ShopId,
                    CouponId = x.CouponId,
                    DiscountPrice = (int)x.DiscountPrice,
                    Title = x.Title,
                    Description = x.Description,
                    TimeSpan = x.TimeSpan,
                    RestTime = x.RemainDays,
                    Status = x.Status
                }),
                CouponsForVoucher = couponDto.CouponsForVoucher.Select(x => new CouponDetail
                {
                    BrandLogo = x.BrandLogo,
                    ShopName = x.ShopName,
                    ShopId = x.ShopId,
                    CouponId = x.CouponId,
                    DiscountPrice = (int)x.DiscountPrice,
                    Title = x.Title,
                    Description = x.Description,
                    TimeSpan = x.TimeSpan,
                    RestTime = x.RemainDays,
                    Status = x.Status
                })
            };

            ViewData["code"] = code;
            return View(result);
        }

        [HttpGet]
        [Route("/RequestForCoupon/{code}")]
        public IActionResult RequestForCoupon(string code)
        {
            var userId = int.Parse(User.Identity.Name);
            var couponResultDto = _activityservice.RequestForCoupon(code, userId);

            var result = _mapper.Map<CouponResultViewModel>(couponResultDto);

            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCoupon/{code}")]
        public IActionResult GetCoupon(string code)
        {
            var userId = int.Parse(User.Identity.Name);
            var couponResultDto = _activityservice.GetCoupon(code, userId);

            var result = _mapper.Map<CouponContentVM>(couponResultDto);

            return PartialView("_CouponModalPartial", result);
        }
    }
}
