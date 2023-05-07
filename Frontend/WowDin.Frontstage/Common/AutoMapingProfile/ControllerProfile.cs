using AutoMapper;
using System.Globalization;
using WowDin.Frontstage.Models.Dto.Activity;
using WowDin.Frontstage.Models.Dto.Order;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.ViewModel.Member;
using WowDin.Frontstage.Models.ViewModel.Store;

namespace WowDin.Frontstage.Common.AutoMapingProfile
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            #region StoreShopMenuViewModel
            CreateMap<MethodDto, MethodVM>();
            CreateMap<SelectionDto, SelectionVM>();
            CreateMap<CustomDto, CustomVM>();
            CreateMap<ProductDto, ProductVM>();
            CreateMap<ProductClassDto, MenuClassVM>();
            CreateMap<FigureDto, FigureVM>();
            CreateMap<PromotionDto, PromotionVM>();
            CreateMap<ProductInCartDto, ProductInCart>()
                .ForMember(x => x.TotalPrice, y => y.MapFrom(o => o.TotalPrice.ToString("C0", CultureInfo.CreateSpecificCulture("zh-TW"))))
                .ForMember(x => x.IsLeader, y => y.Ignore())
                .ForMember(x => x.MsgForGroup, y => y.Ignore())
                .ReverseMap();
            CreateMap<ShopMenuDto, StoreShopMenuViewModel>()
                .ForMember(x => x.IsOpen, y => y.Ignore())
                .ForMember(x => x.Span, y => y.Ignore())
                .ForMember(x => x.ShopId, y => y.Ignore())
                .ForMember(x => x.FullAddress, y => y.MapFrom(o => o.MapData.FullAddress))
                .ForMember(x => x.PayMethod, y => y.MapFrom(o => "可" + string.Join('、', o.PayMethod)))
                .ForMember(x => x.ProductDetailModal, y => y.Ignore())
                .ForMember(x => x.Star, y => y.MapFrom(o => (o.Star / o.StarAmount).ToString()))
                .ReverseMap();
            #endregion

            #region Coupon
            CreateMap<CouponContentDto, CouponContentVM>().ReverseMap();
            CreateMap<CouponResultDto, CouponResultViewModel>().ReverseMap();
            #endregion

            #region OrderDeliveryFeeVM
            CreateMap<DeliveryFeeDto, DeliveryFeeViewModel>();
            #endregion
        }
    }
}
