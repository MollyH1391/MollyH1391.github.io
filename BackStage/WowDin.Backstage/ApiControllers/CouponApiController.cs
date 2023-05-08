using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WowDin.Backstage.Models.Base;
using WowDin.Backstage.Models.Dto.Coupon;
using WowDin.Backstage.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WowDin.Backstage.Controllers
{
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CouponApiController(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [Authorize]
        [Route("api/GetAllCoupon/{brandId}")]
        [HttpGet]
        public IActionResult GetAllCouponOfBrand(int brandId)
        {
            var allCouponDto = _couponService.GetAllCouponOfBrand(brandId);

            var result = _mapper.Map<AllCouponVM>(allCouponDto);

            return Ok(new APIResult(APIStatus.Success, "讀取成功", allCouponDto));
        }

        [Authorize]
        [Route("api/GetShopsAndProducts/{brandId}")]
        [HttpGet]
        public IActionResult GetShopsAndProducts(int brandId)
        {
            var shopsForCouponDto = _couponService.GetShopsAndProducts(brandId);

            var result = _mapper.Map<ShopsForCouponVM>(shopsForCouponDto);

            return Ok(new APIResult(APIStatus.Success, "讀取成功", result));
        }

        [Authorize]
        [Route("api/Coupon")]
        [HttpPost]
        public IActionResult CreateCoupon(CreateCouponVM request)
        {
            var createCouponDto = _mapper.Map<CreateCouponDto>(request);
            var result = new APIResult(APIStatus.Success, "新增成功", null);

            var code = $"C{DateTimeOffset.Now.ToUnixTimeSeconds()}";

            foreach (var shop in request.CouponBelong)
            {
                createCouponDto.CouponBelong = _mapper.Map<ShopAndProductDto>(shop);
                var tempResult = _couponService.CreateCoupon(createCouponDto, code);

                if (tempResult.Status == APIStatus.Fail) { 
                    result = new APIResult(APIStatus.Fail, "新增失敗", null);
                }
            }

            return Ok(result);
        }

        [Authorize]
        [Route("api/Coupon")]
        [HttpPut]
        public IActionResult EditCoupon(EditCouponDto request)
        {
            var editCouponDto = _mapper.Map<EditCouponDto>(request);

            var result = _couponService.EditCoupon(editCouponDto);

            return Ok(result);
        }

        [Authorize]
        [Route("api/Coupon/{couponId}")]
        [HttpGet]
        public IActionResult SwitchCouponStatus(int couponId)
        {
            var result = _couponService.SwitchCouponStatus(couponId);

            return Ok(result);
        }

        [Authorize]
        [Route("api/SwitchAll")]
        
        [HttpPost]
        public IActionResult AutoSwitchStatus(IEnumerable<int> couponIds)
        {
            bool success = true;
            
            foreach(var couponId in couponIds)
            {
                var tempResult = _couponService.SwitchCouponStatus(couponId);
                if(tempResult.Status == APIStatus.Fail)
                {
                    success = false;
                }
            }

            var result = success ? new APIResult(APIStatus.Success, $"已將{couponIds.Count()}個已過期的優惠關閉", null) : new APIResult(APIStatus.Fail, $"保存失敗", null);

            return Ok(result);
        }
    }
}
