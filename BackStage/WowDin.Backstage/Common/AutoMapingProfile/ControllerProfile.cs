using AutoMapper;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.ViewModel.Menu;

namespace WowDin.Frontstage.Common.AutoMapingProfile
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            #region MenuViewModel
            CreateMap<SelectionDto, SelectionVM>().ReverseMap();
            CreateMap<CustomDto, CustomVM>().ReverseMap();
            CreateMap<ProductDto, ProductVM>().ReverseMap();
            CreateMap<ProductClassDto, MenuClassVM>().ReverseMap();
            CreateMap<GetMenuDataDto, MenuViewModel>().ReverseMap();
            #endregion
        }
    }
}
