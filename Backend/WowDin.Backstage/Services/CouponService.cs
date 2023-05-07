using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Backstage.Common.ModelEnum;
using WowDin.Backstage.Models.Base;
using WowDin.Backstage.Models.Dto.Coupon;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Services
{
    public class CouponService : ICouponService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponService> _logger;

        public CouponService(IRepository repository, IMapper mapper, ILogger<CouponService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public AllCouponDto GetAllCouponOfBrand(int brandId)
        {
            var shops = _repository.GetAll<Shop>().Where(x => x.BrandId == brandId).ToList();
            var coupons = _repository.GetAll<Coupon>().Where(x => shops.Select(s => s.ShopId).Contains(x.ShopId)).ToList();
            var couponProducts = _repository.GetAll<CouponProduct>().Where(x => coupons.Select(c => c.CouponId).Contains(x.CouponId)).ToList();
            var allProducts = _repository.GetAll<Product>().Where(x => couponProducts.Select(p => p.ProductId).Contains(x.ProductId)).ToList();

            var result = new AllCouponDto()
            {
                Coupons = new List<CouponDetailDto>()
            };
            
            foreach (var coupon in coupons)
            {
                var couponDetail = _mapper.Map<CouponDetailDto>(coupon);
                
                if(coupon.ThresholdType == (int)CouponEnum.CouponType.ForProduct)
                {
                    var products = allProducts.Where(x => couponProducts.Where(c => c.CouponId == coupon.CouponId).Select(c => c.ProductId).Contains(x.ProductId));
                    couponDetail.Products = String.Join('、', products.Select(x => x.Name));
                }
                
                couponDetail.ShopName = shops.First(x => x.ShopId == coupon.ShopId).Name;
                couponDetail.ReleasedAmount = CalculateReleasedCoupon(coupon.CouponId);
                result.Coupons.Add(couponDetail);
            }

            return result;
        }

        private int CalculateReleasedCoupon(int couponId)
        {
            return _repository.GetAll<CouponContainer>().Count(x => x.CouponId == couponId);
        }

        public ShopsForCouponDto GetShopsAndProducts(int brandId)
        {
            var shops = _repository.GetAll<Shop>().Where(x => x.BrandId == brandId && x.State != (int)ShopEnum.StateEnum.Remove).ToList();
            var classes = _repository.GetAll<MenuClass>().Where(x => shops.Select(s => s.ShopId).Contains(x.ShopId)).ToList();
            var products = _repository.GetAll<Product>().Where(x => classes.Select(s => s.MenuClassId).Contains(x.MenuClassId) && x.State!=(int)ProductEnum.StateEnum.Deleted).ToList();

            var result = shops.Select(s => new ShopAndProductsDto
            {
                ShopId = s.ShopId,
                ShopName = s.Name,
                Products = products.Where(p => classes.Where(c => c.ShopId == s.ShopId).Select(c => c.MenuClassId).Contains(p.MenuClassId)).Select(p => new ProductOfShopDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name
                })
            });

            return new ShopsForCouponDto { Shops = result };
        }

        public APIResult CreateCoupon(CreateCouponDto request, string code)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var couponToCreate = _mapper.Map<Coupon>(request);
                    couponToCreate.Code = code;
                    couponToCreate.ShopId = request.CouponBelong.ShopId;

                    var coupon = _repository._context.Coupons.Add(couponToCreate);
                    _repository._context.SaveChanges();

                    if (request.Type == (int)CouponEnum.CouponType.ForProduct && request.CouponBelong.ProductIds.Count() != 0)
                    {
                            
                        foreach(var productId in request.CouponBelong.ProductIds)
                        {
                            var couponProduct = new CouponProduct
                            {
                                CouponId = coupon.Entity.CouponId,
                                ProductId = productId
                            };
                            _repository.Create(couponProduct);
                            _repository.Save();
                        }
                    }
                    transaction.Commit();
                    return new APIResult(APIStatus.Success, "新增成功", null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "優惠券新增失敗");
                    return new APIResult(APIStatus.Fail, "新增失敗", null);
                }
            }
        }

        public APIResult EditCoupon(EditCouponDto coupon)
        {
            try 
            {
                var target = _repository.GetAll<Coupon>().First(x => x.CouponId == coupon.Id);
                target.CouponName = coupon.CouponName;
                target.StartTime = coupon.StartTime;
                target.EndTime = coupon.EndTime;
                target.MaxAmount = coupon.MaxAmount;
                target.Description = coupon.Description;
                target.Code = coupon.Code;

                _repository.Update(target);
                _repository.Save();
                
                return new APIResult(APIStatus.Success, "保存成功", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "優惠券編輯失敗");
                return new APIResult(APIStatus.Fail, "保存失敗", null);
            }
        }

        public APIResult SwitchCouponStatus(int couponId)
        {
            try
            {
                var coupon = _repository.GetAll<Coupon>().First(x => x.CouponId == couponId);
                if (coupon.Status == (int)CouponEnum.CouponStatus.Available)
                {
                    coupon.Status = (int)CouponEnum.CouponStatus.Unavailable;
                }
                else
                {
                    coupon.Status = (int)CouponEnum.CouponStatus.Available;
                }

                _repository.Update(coupon);
                _repository.Save();
                return new APIResult(APIStatus.Success, "保存成功", null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "優惠券狀態切換失敗");
                return new APIResult(APIStatus.Fail, "保存失敗", null);
            }
        }
        
    }
}
