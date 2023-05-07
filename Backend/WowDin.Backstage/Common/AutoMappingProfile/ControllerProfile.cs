using AutoMapper;
using System.Linq;
using WowDin.Backstage.Models.Dto.Information;
using WowDin.Backstage.Models.Dto.Coupon;
using WowDin.Backstage.Models.Dto.Member;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.ViewModel.Information;
using WowDin.Backstage.Models.ViewModel.Member;
using WowDin.Backstage.Models.ViewModel.Menu;
using WowDin.Backstage.Models.Dto.Advertise;

namespace WowDin.Backstage.Common.AutoMapingProfile
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            #region MenuViewModel
            CreateMap<SelectionDto, SelectionVM>()
                .ForMember(x => x.Id, y => y.MapFrom(o => o.Id.ToString()))
                .ReverseMap();
            CreateMap<CustomDto, CustomVM>()
                .ForMember(x => x.Id, y => y.MapFrom(o => o.Id.ToString()))
                .ReverseMap();
            CreateMap<ProductDto, ProductVM>()
                .ForMember(x => x.Id, y => y.MapFrom(o => o.Id.ToString()))
                .ForMember(x => x.UploadFigure, y => y.Ignore())
                .ReverseMap();
            CreateMap<MenuClassDto, MenuClassVM>().ReverseMap();
            CreateMap<GetMenuDataDto, MenuVM>().ReverseMap();
            #endregion

            #region MemberViewModel
            CreateMap<GetCardGradingDto, MemberCardGradingViewModel>();
            CreateMap<CreateCardCradingVM, CreateCardCradingDto>();
            CreateMap<DeleteCardVM, DeleteCardDto>();
            CreateMap<UpdateCardVM, UpdateCardDto>();
            CreateMap<GetBrandMemberDataDto, MemberPointViewModel>();
            #endregion

            #region InformationVM
            CreateMap<BrandFacadeDto, BrandFacadeViewModel>()
                .ForMember(x => x.BrandCategories, y => y.MapFrom(o => o.BrandCategories.Select(c => new BrandManagementViewModel.Category
                {
                    Id = c.Id,
                    Image = c.Image
                }).ToList()));
            CreateMap<BrandFacadeViewModel, BrandFacadeDto>()
                .ForMember(x => x.BrandCategories, y => y.MapFrom(o => o.BrandCategories.Select(c => new BrandManagementDto.Category
                {
                    Id = c.Id,
                    Image = c.Image
                })));
            CreateMap<BrandIntroduceDto, BrandIntroduceViewModel>().ReverseMap();
            CreateMap<BrandWebDto, BrandWebViewModel>().ReverseMap();
            CreateMap<BrandImagesDto, BrandImagesViewModel>()
                .ForMember(x => x.Images, y => y.MapFrom(o => o.Images.Select(i => new BrandImagesViewModel.Image
                {
                    Id = i.Id,
                    ImageAlt = i.ImageAlt,
                    ImageUrl = i.ImageUrl,
                    Sort = i.Sort
                }).ToList()));
            CreateMap<BrandImagesViewModel, BrandImagesDto>()
                .ForMember(x => x.Images, y => y.MapFrom(o => o.Images.Select(i => new BrandImagesDto.Image
                {
                    Id = i.Id,
                    ImageAlt = i.ImageAlt,
                    ImageUrl = i.ImageUrl,
                    Sort = i.Sort
                })));
            CreateMap<ShopImagesDto, ShopImagesViewModel>()
                .ForMember(x => x.Images, y => y.MapFrom(o => o.Images.Select(i => new ShopImagesViewModel.Image
                {
                    Id = i.Id,
                    ImageAlt = i.ImageAlt,
                    ImageUrl = i.ImageUrl,
                    Sort = i.Sort
                }).ToList()));
            CreateMap<ShopImagesViewModel, ShopImagesDto>()
                .ForMember(x => x.Images, y => y.MapFrom(o => o.Images.Select(i => new ShopImagesDto.Image
                {
                    Id = i.Id,
                    ImageAlt = i.ImageAlt,
                    ImageUrl = i.ImageUrl,
                    Sort = i.Sort
                })));
            CreateMap<ShopInfoDto, ShopInfoViewModel>()
                .ForMember(x => x.HasSticker, y => y.MapFrom(o => o.HasSticker.ToString()))
                .ForMember(x => x.PriceLimit, y => y.MapFrom(o => o.PriceLimit.ToString()))
                .ForMember(x => x.PaymentTypes, y => y.MapFrom(o => o.PaymentTypes.ToList())).ReverseMap();
            CreateMap<ShopBusinessDto, ShopBusinessViewModel>()
                .ForMember(x => x.OpenDayList, y => y.MapFrom(o => o.OpenDayList.ToList())).ReverseMap();
            CreateMap<ShopTakeMethodDto, ShopTakeMethodViewModel>()
                .ForMember(x=>x.TakeOutTime, y => y.MapFrom(o => o.TakeOutTime.ToString()))
                .ForMember(x => x.DeliveryConditions, y => y.MapFrom(o => o.DeliveryConditions.Select(d => new ShopTakeMethodViewModel.DeliveryCondition
                {
                    Id = d.Id,
                    PriceThreshold = d.PriceThreshold.ToString("0"),
                    LowerDistance = d.LowerDistance.ToString(),
                    HigherDistance = d.HigherDistance.ToString(),
                    DeliveryTime = d.DeliveryTime.ToString(),
                    DeliveryFee = d.DeliveryFee.ToString("0"),
                    _showDetails = false
                }).ToList()));
            CreateMap<ShopTakeMethodViewModel, ShopTakeMethodDto>()
                .ForMember(x=>x.TakeOutTime, y => y.MapFrom(o => double.Parse(o.TakeOutTime)))
                .ForMember(x => x.DeliveryConditions, y => y.MapFrom(o => o.DeliveryConditions.Select(d => new ShopTakeMethodDto.DeliveryCondition
                {
                    Id = d.Id,
                    PriceThreshold = decimal.Parse(d.PriceThreshold),
                    LowerDistance = double.Parse(d.LowerDistance),
                    HigherDistance = string.IsNullOrEmpty(d.HigherDistance)?null:double.Parse(d.HigherDistance),
                    DeliveryTime = double.Parse(d.DeliveryTime),
                    DeliveryFee = decimal.Parse(d.DeliveryFee),
                })));
            CreateMap<UploadImageDto, UploadImageViewModel>();
            CreateMap<SaveDto, SaveViewModel>()
                .ForMember(x => x.NewShop, y => y.MapFrom(o => o.NewShop == null ? null : new SaveViewModel.Shop { text=o.NewShop.Name,value=o.NewShop.Id}));
            CreateMap<ShopManagementDto, ShopManagementViewModel>()
                .ForMember(x => x.Shops, y => y.MapFrom(o => o.Shops.Select(s => new ShopManagementViewModel.Shop { value = s.Id, text = s.Name }).ToList()))
                .ForMember(x => x.States, y => y.MapFrom(o => o.States.Select(s => new ShopManagementViewModel.State { value = s.Id, text = s.Name }).ToList()))
                .ForMember(x => x.PaymentTypes, y => y.MapFrom(o => o.PaymentTypes.Select(p => new ShopManagementViewModel.PaymentType { value = p.Id, text = p.Name }).ToList()))
                .ForMember(x => x.DayList, y => y.MapFrom(o => o.DayList.Select(d => new ShopManagementViewModel.Day { value = d.Value, text = d.Text }).ToList()))
                .ForMember(x => x.CityDistrict, y => y.MapFrom(o => o.CityDistrict.Select(c => new CityDistrictViewModel { City = c.City, District = c.District }).ToList()));
            #endregion

            #region CouponVM
            CreateMap<AllCouponDto, AllCouponVM>().ReverseMap();
            CreateMap<CouponDetailDto, CouponDetailVM>().ReverseMap();
            CreateMap<CreateCouponDto, CreateCouponVM>().ReverseMap()
                .ForMember(x => x.CouponBelong, y => y.Ignore());
            CreateMap<ShopsForCouponDto, ShopsForCouponVM>().ReverseMap();
            CreateMap<ShopAndProductsDto, ShopAndProductsVM>().ReverseMap();
            CreateMap<ProductOfShopDto, ProductOfShopVM>().ReverseMap();
            CreateMap<EditCouponDto, EditCouponVM>().ReverseMap();
            CreateMap<ShopAndProductDto, ShopAndProductVM>().ReverseMap();
            #endregion

            #region AdvertiseVM
            CreateMap<AllAdvertiseDto, AllAdvertiseVM>().ReverseMap();
            CreateMap<AdvertiseDto, AdvertiseVM>().ReverseMap();
            CreateMap<AdvertiseRequestDto, AdvertiseRequestVM>().ReverseMap();
            #endregion
        }
    }
}
