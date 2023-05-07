using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Models.Dto.PartialView;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.ViewModel.PartialView;
using WowDin.Frontstage.Models.ViewModel.Store;

namespace WowDin.Frontstage.Services
{
    public static class TransformService
    {
        public static List<ShopCardViewModel> ShopCardDtoTransToShopCardVM(this IEnumerable<ShopCardDto> shopCardDto)
        {
            if (shopCardDto != null)
            {
                return shopCardDto.Select(x => new ShopCardViewModel
                {
                    ShopId = x.ShopId,
                    ShopName = x.ShopName,
                    ShopFig = x.ShopFig,
                    BrandLogo = x.BrandLogo,
                    BrandName = x.BrandName,
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime,
                    Distance = x.Distance.ToString("0.0"),
                    Star = x.Star.ToString("0.0"),
                    Address = x.Address,
                    IsDelivery = x.IsDelivery,
                    IsFavorite = x.IsFavorite,
                    Categories = x.Categories.Select(y => new ShopCardViewModel.Category { CategoryFig = y.CategoryFig, CategoryName = y.CategoryName }).ToList()
                }).ToList();
            }
            return null;
        }
        public static SearchRequestDto SearchReDMTransToSearchReDto(this SearchDataModel request)
        {
            if (request != null)
            {
                return new SearchRequestDto
                {
                    Method = request.Method,
                    Lat = request.Lat,
                    Lng = request.Lng,
                    Address = request.Address,
                    Brand = request.Brand,
                    Category = request.Category,
                    Evaluate = request.Evaluate
                };
            }
            return null;
        }
        public static SearchViewModel DtoTransToSearchVM(SearchDataModel request, SearchDto searchDto, IEnumerable<ShopCardDto> shopCards)
        {
            return new SearchViewModel
            {
                Method = request.Method,
                Lat = request.Lat,
                Lng = request.Lng,
                Address = request.Address,
                SelectedBrand = request.Brand,
                SelectedCategory = request.Category,
                SelectedEvaluate = request.Evaluate,
                Brands = searchDto.Brands.ToList(),
                Categories = searchDto.Categories.Select(x => new SearchViewModel.Category { Name = x.Name, Img = x.Img }).ToList(),
                ShopCards = shopCards.ShopCardDtoTransToShopCardVM()
            };
        }
    }
}
