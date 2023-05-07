using AutoMapper;
using CoreMVC_Project.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models.Dto;
using WowDin.Frontstage.Models.Dto.Activity;
using WowDin.Frontstage.Models.Dto.Home;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Repositories.Interface;

namespace WowDin.Frontstage.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public ActivityService(IRepository repository, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
        }
        public GetCouponDto GetCouponByUserId(int id)
        {
            var couponsOfUser = _repository.GetAll<CouponContainer>().Where(x => x.UserAccountId == id).ToList();
            var coupons = _repository.GetAll<Coupon>().Where(c => couponsOfUser.Select(x => x.CouponId).Contains(c.CouponId) && c.Status == (int)CouponEnum.CouponStatus.Available).ToList();
            Dictionary<int, ShopData> shopDatas = new Dictionary<int, ShopData>();
            foreach(var coupon in coupons)
            {
                var shop = _repository.GetAll<Shop>().First(x => x.ShopId == coupon.ShopId);
                var brand = _repository.GetAll<Brand>().First(x => x.BrandId == shop.BrandId);

                shopDatas.Add(coupon.CouponId, new ShopData { 
                    ShopId = shop.ShopId, 
                    ShopName = shop.Name, 
                    BrandId = brand.BrandId, 
                    BrandName = brand.Name, 
                    BrandLogo = brand.Logo
                });
            }
            var today = DateTime.Now;
            var result = new GetCouponDto()
            {
                Brands = coupons.Select(c => _repository.GetAll<Shop>().First(s => s.ShopId == c.ShopId)).Select(x => _repository.GetAll<Brand>().First(b => b.BrandId == x.BrandId).Name).Distinct(),
                Status = new List<string> { Extensions.GetDescription(CouponContainerEnum.CouponState.All), Extensions.GetDescription(CouponContainerEnum.CouponState.Usable), Extensions.GetDescription(CouponContainerEnum.CouponState.Used), Extensions.GetDescription(CouponContainerEnum.CouponState.Expired) },
                CouponsForDiscount = coupons.Where(c => c.ThresholdType == 2).Select(x => new CouponDto
                {
                    BrandLogo = shopDatas[x.CouponId].BrandLogo,
                    ShopName = shopDatas[x.CouponId].BrandName + "-" + shopDatas[x.CouponId].ShopName,
                    ShopId = x.ShopId,
                    CouponId = x.CouponId,
                    DiscountPrice = x.DiscountAmount,
                    Title = x.CouponName,
                    Description = x.Description,
                    TimeSpan = $"{x.StartTime.ToString("yyyy/MM/dd")}-{x.EndTime.ToString("yyyy/MM/dd")}",
                    RemainDays = (x.EndTime - today).TotalDays >= 0 ? "剩" + (x.EndTime - today).Days + "天" : "",
                    Status = (x.EndTime - today).TotalDays > 0 ? Extensions.GetDescription((CouponContainerEnum.CouponState)couponsOfUser.First(y => y.CouponId == x.CouponId).CouponState) : "已過期"
                }),
                CouponsForVoucher = coupons.Where(c => c.ThresholdType == 1).Select(x => new CouponDto
                {
                    BrandLogo = shopDatas[x.CouponId].BrandLogo,
                    ShopName = shopDatas[x.CouponId].BrandName + "-" + shopDatas[x.CouponId].ShopName,
                    ShopId = x.ShopId,
                    CouponId = x.CouponId,
                    DiscountPrice = x.DiscountAmount,
                    Title = x.CouponName,
                    Description = x.Description,
                    TimeSpan = $"{x.StartTime.ToString("yyyy/MM/dd")}-{x.EndTime.ToString("yyyy/MM/dd")}",
                    RemainDays = (x.EndTime - today).TotalDays >= 0 ? "剩" + (x.EndTime - today).Days + "天" : "",
                    Status = (x.EndTime - today).TotalDays > 0 ? Extensions.GetDescription((CouponContainerEnum.CouponState)couponsOfUser.First(y => y.CouponId == x.CouponId).CouponState) : "已過期"
                })
            };
            return result;
        }

        public GetIndexFiguresDto GetIndexFigures()
        {
            var indexFiguresDto = _memoryCacheRepository.Get<GetIndexFiguresDto>("Activity.GetIndexFigures");

            if(indexFiguresDto == null)
            {
                var brandCheck = _repository.GetAll<Brand>().Where(x => x.Verified == 1 && x.Suspension == false);
                var brandData = _repository.GetAll<Brand>().Where(x => brandCheck.Select(b => b.BrandId).Contains(x.BrandId)).ToList();
                var ads = _repository.GetAll<Advertise>().Where(ad => brandCheck.Select(b => b.BrandId).Contains(ad.BrandId) && ad.Status == 1).ToList();
                var couponList = _repository.GetAll<Coupon>().ToList();
                indexFiguresDto = new GetIndexFiguresDto
                {
                    BigPictures = ads.Where(a => a.IsSearchZone == true).Select(a => new PictureDto
                    {
                        PictureFile = a.Img,
                        BrandId = a.BrandId,
                        Code = couponList.FirstOrDefault(x => x.CouponId == a.CouponId)?.Code

                    }),
                    SmallPictures = ads.Where(a => a.IsSearchZone == false).Select(a => new PictureDto
                    {
                        PictureFile = a.Img,
                        BrandId = a.BrandId,
                        Code = couponList.FirstOrDefault(x => x.CouponId == a.CouponId)?.Code
                    }).Take(3),
                };

                _memoryCacheRepository.Set("Activity.GetIndexFigures", indexFiguresDto);
                //_memoryCacheRepository.Set("IndexFigures", indexFiguresDto, DateTimeOffset.Now.AddHours(1));
            }
            return indexFiguresDto;
        }

        public IEnumerable<UsableCouponByUserDto> GetUsableCouponByUser(int userId, int shopId)
        {
            UpdateExpiredCoupon(shopId);

            var userCouponContainer = _repository.GetAll<CouponContainer>()
                .Where(x => x.UserAccountId == userId && 
                x.CouponState == (int)CouponContainerEnum.CouponState.Usable)
                .Select(x => x.CouponId).ToList();

            var couponList = _repository.GetAll<Coupon>()
                .Where(x => userCouponContainer.Contains(x.CouponId) && 
                x.ShopId == shopId);

            return couponList.Select(x => new UsableCouponByUserDto
            {
                CouponId = x.CouponId,
                CouponTitle = x.CouponName
            });
        }

        private void UpdateExpiredCoupon(int shopId)
        {
            var today = DateTime.UtcNow;
            var couponsOfShop = _repository.GetAll<Coupon>().Where(x => x.ShopId == shopId).ToList();
            var expiredCouponsOfShop = couponsOfShop.Where(x => (today - x.EndTime).TotalDays > 0)
                                        .Select(x => x.CouponId);
            
            if(expiredCouponsOfShop.Count() > 0)
            {
                var couponsOfUser = _repository.GetAll<CouponContainer>()
                                    .Where(x => expiredCouponsOfShop.Contains(x.CouponId)).ToList();
            
                foreach (var coupon in couponsOfUser)
                {
                    coupon.CouponState = (int)CouponContainerEnum.CouponState.Expired;

                    _repository.Update(coupon);
                    _repository.Save();
                }
            }
        }

        public CalculateCouponDiscountDto CalculateCouponDiscount(int cartId, int couponId)
        {
            var cartDetails = _repository.GetAll<CartDetail>().Where(x => x.CartId == cartId).ToList();
            var result = new CalculateCouponDiscountDto() { IsValid = false, Discount = 0 };
            if(couponId == 0)
            {
                return result;
            }
            var coupon = _repository.GetAll<Coupon>().First(x => x.CouponId == couponId);

            switch (coupon.ThresholdType)
            {
                case (int)CouponEnum.CouponType.ForProduct: //商品折扣
                    var productId = _repository.GetAll<CouponProduct>().First(x => x.CouponId == couponId).ProductId;
                    if (cartDetails.Count > 1 && cartDetails.Select(x => x.ProductId).Contains(productId))
                    {
                        result.IsValid = true;
                        result.Discount = coupon.DiscountAmount;
                    }
                    break;
                case (int)CouponEnum.CouponType.Storewide: //優惠打折
                    var totalPrice = cartDetails.Select(x => x.UnitPrice * x.Quantity).Sum();
                    if (totalPrice > coupon.ThresholdAmount)
                    {
                        result.IsValid = true;
                        result.Discount = Math.Ceiling(totalPrice * (1 - coupon.DiscountAmount));
                    }
                    break;
                default:
                    result.IsValid = false;
                    result.Discount = 0;
                    break;
            }

            return result;
        }

        public CouponResultDto RequestForCoupon (string code, int userId)
        {
            var result = new CouponResultDto()
            {
                IsSuccess = false,
                Message = "無此折價券"
            };

            var today = DateTime.Now;
            var coupon =  _repository.GetAll<Coupon>().FirstOrDefault(x => x.Code == code && 
                            x.EndTime > today && x.Status == (int)CouponEnum.CouponStatus.Available);
            var couponOfUser = _repository.GetAll<CouponContainer>().Where(x => x.UserAccountId == userId)
                                .Select(x => x.CouponId).ToList();

            if(coupon == null)
            {
                return result;
            }
            else
            {
                if (couponOfUser.Contains(coupon.CouponId))
                {
                    result.Message = "您已擁有此折價券";
                    return result;
                }

                result.IsSuccess = true;

                result.Message = "成功兌換! 請至「我的券夾」查看";
            }
            return result;
        }

        public CouponContentDto GetCoupon(string code, int userId)
        {
            var today = DateTime.Now;
            var coupons = _repository.GetAll<Coupon>().Where(x => x.Code == code && x.EndTime > today && x.Status == (int)CouponEnum.CouponStatus.Available).ToList(); 
            var shops = _repository.GetAll<Shop>().Where(s => coupons.Select(c => c.ShopId).Contains(s.ShopId)).ToList();
            var brand = _repository.GetAll<Brand>().First(x => x.BrandId == shops[0].BrandId);

            var result = new CouponContentDto()
            {
                BrandLogo = brand.Logo,
                BrandName = brand.Name,
                Type = Extensions.GetDescription((CouponEnum.CouponType)coupons[0].ThresholdType),
                Title = coupons[0].CouponName,
                Description = coupons[0].Description,
                TimeSpan = coupons[0].StartTime.ToString("yyyy/MM/dd") + '~' + coupons[0].EndTime.ToString("yyyy/MM/dd"),
                RestTime = (coupons[0].EndTime - today).Days,
                Shops = shops.Select(s => s.Name)
            };

            foreach (var coupon in coupons)
            {
                
                var entity = new CouponContainer() { 
                    UserAccountId = userId, 
                    CouponId = coupon.CouponId, 
                    CouponState = (int)CouponContainerEnum.CouponState.Usable 
                };

                _repository.Create(entity);
                _repository.Save();

                if (IsCouponReleasedAll(coupon.CouponId))
                {
                    coupon.Status = (int)CouponEnum.CouponStatus.Unavailable;
                    _repository.Update(coupon);
                    _repository.Save();
                }
            }

            return result;
        }


        private bool IsCouponReleasedAll(int couponId)
        {
            var target = _repository.GetAll<Coupon>().First(x => x.CouponId == couponId);
            var amount = _repository.GetAll<CouponContainer>().Count(x => x.CouponId == couponId);

            return amount >= target.MaxAmount;
        }
    }
}
