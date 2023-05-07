using AutoMapper;
using WowDin.Backstage.Common.ModelEnum;
using WowDin.Backstage.Models.Dto.Advertise;
using WowDin.Backstage.Models.Dto.Coupon;
using WowDin.Backstage.Models.Dto.Member;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Models.ViewModel.Menu;

namespace WowDin.Backstage.Common.AutoMapingProfile
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            #region Menu
            CreateMap<MenuClass, MenuClassDto>()
                .ForMember(x => x.Products, y => y.Ignore())
                .ReverseMap()
                .ForMember(x => x.ClassName, y => y.MapFrom(o => o.Name))
                .ForMember(x => x.ShopId, y => y.Ignore())
                .ForMember(x => x.MenuClassId, y => y.MapFrom(o => o.Id));
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Customs, y => y.Ignore())
                .ReverseMap()
                .ForMember(x => x.ProductId, y => y.MapFrom(o => o.Id))
                .ForMember(x => x.MenuClassId, y => y.Ignore())
                .ForMember(x => x.Fig, y => y.MapFrom(o => o.Figure))
                .ForMember(x => x.Note, y => y.MapFrom(o => o.ChangeNote));
            CreateMap<Custom, CustomDto>()
                .ForMember(x => x.Selections, y => y.Ignore())
                .ReverseMap()
                .ForMember(x => x.CustomId, y => y.MapFrom(o => o.Id))
                .ForMember(x => x.ProductId, y => y.Ignore());
            CreateMap<CustomSelection, SelectionDto>().ReverseMap()
                .ForMember(x => x.CustomSelectionId, y => y.Ignore())
                .ForMember(x => x.CustomId, y => y.MapFrom(o => o.Id))
                .ForMember(x => x.AddPrice, y => y.MapFrom(o => o.Price));

            CreateMap<ProductVM, ProductInputVM>()
                .ForMember(x => x.ShopId, y => y.Ignore())
                .ForMember(x => x.MenuClassId, y => y.Ignore())
                .ReverseMap();
            #endregion

            #region Member
            CreateMap<CardType, GetCardGradingDto>();
            CreateMap<CreateCardCradingDto, CardType>();
            CreateMap<UpdateCardDto, CardType>();
            #endregion

            #region Coupon
            CreateMap<Coupon, CouponDetailDto>()
                .ForMember(x => x.Id, y => y.MapFrom(o => o.CouponId))
                .ForMember(x => x.ShopName, y => y.Ignore())
                .ForMember(x => x.Products, y => y.Ignore())
                .ForMember(x => x.Type, y => y.MapFrom(o => o.ThresholdType))
                .ForMember(x => x.ReleasedAmount, y => y.Ignore())
                .ForMember(x => x.Status, y => y.MapFrom(o => Extensions.GetDescription((CouponEnum.CouponStatus)o.Status)))
                .ForMember(x => x.StartTime, y => y.MapFrom(o => o.StartTime.ToString("yyyy/MM/dd")))
                .ForMember(x => x.EndTime, y => y.MapFrom(o => o.EndTime.ToString("yyyy/MM/dd")))
                .ForMember(x => x.MaxAmount, y => y.MapFrom(o => o.MaxAmount == null ? 0 : o.MaxAmount))
                .ReverseMap()
                .ForMember(x => x.DiscountType, y => y.MapFrom(o => o.Type))
                .ForMember(x => x.Description, y => y.MapFrom(o => o.Description == null ? o.CouponName : o.Description));
            CreateMap<Coupon, CreateCouponDto>()
                .ForMember(x => x.CouponBelong, y => y.Ignore())
                .ForMember(x => x.Type, y => y.MapFrom(o => o.ThresholdType))
                .ReverseMap()
                .ForMember(x => x.Code, y => y.Ignore())
                .ForMember(x => x.DiscountType, y => y.MapFrom(o => o.Type))
                .ForMember(x => x.ShopId, y => y.Ignore());
            CreateMap<Coupon, EditCouponDto>()
                .ForMember(x => x.Id, y => y.MapFrom(o => o.CouponId))
                .ReverseMap()
                .ForMember(x => x.ShopId, y => y.Ignore())
                .ForMember(x => x.ThresholdType, y => y.Ignore())
                .ForMember(x => x.ThresholdAmount, y => y.Ignore())
                .ForMember(x => x.DiscountType, y => y.Ignore())
                .ForMember(x => x.DiscountAmount, y => y.Ignore())
                .ForMember(x => x.Status, y => y.Ignore());

            #endregion

            #region Advertise
            CreateMap<Advertise, AdvertiseDto>()
                .ForMember(x => x.Status, y => y.MapFrom(o => Extensions.GetDescription((AdvertiseEnum.StatusEnum)o.Status)))
                .ForMember(x => x.CouponCode, y => y.Ignore())
                .ReverseMap()
                .ForMember(x => x.AdvertiseId, y => y.Ignore())
                .ForMember(x => x.Sort, y => y.Ignore())
                .ForMember(x => x.BrandId, y => y.Ignore());
            CreateMap<Advertise, AdvertiseRequestDto>()
                .ReverseMap()
                .ForMember(x => x.AdvertiseId, y => y.Ignore())
                .ForMember(x => x.Img, y => y.MapFrom(o => o.AdFig))
                .ForMember(x => x.Sort, y => y.Ignore())
                .ForMember(x => x.Status, y => y.Ignore())
                .ForMember(x => x.Message, y => y.Ignore());
            #endregion
        }
    }
}
