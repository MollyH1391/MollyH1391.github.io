using AutoMapper;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.Entity;

namespace WowDin.Frontstage.Common.AutoMapingProfile
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            #region Menu
            CreateMap<MenuClass, ProductClassDto>()
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
            #endregion
        }
    }
}
