using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Models;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.ViewModel.Store;
using WowDin.Frontstage.Services.Store;
using WowDin.Frontstage.Common;
using System.IO;
using System.Text;
using WowDin.Frontstage.Models.ViewModel.PartialView;
using WowDin.Frontstage.Services;
using System.Text.RegularExpressions;
using WowDin.Frontstage.Services.Order.Interface;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Threading.Tasks;
using WowDin.Frontstage.Services.Interface;

namespace WowDin.Frontstage.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public StoreController(IStoreService storeService, IOrderService orderService, IMapper mapper)
        {
            _storeService = storeService;
            _mapper = mapper;
            _orderService = orderService;
        }

        public IActionResult Test()
        {
            //string path = @"wwwroot/json/CityDistrict.json";
            //string test = System.IO.File.ReadAllText(path, Encoding.GetEncoding("gb2312"));
            //ViewData["test"] = Extensions.JsonSerialize(test);

            return View();
        }

        [Route("/BrandStore/{brandId}")]
        public IActionResult BrandStore(int brandId)
        {
            if (!_storeService.BrandExist(brandId))
            {
                TempData["ErrorMessage"] = "此品牌暫不服務，請選擇其他品牌，感謝!";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }

            if (brandId == 0)
            {
                TempData["ErrorMessage"] = "找不到此頁面";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }
            else
            {
                var brandData = _storeService.GetBrandDataById(brandId);
                var brandDataList = _storeService.GetAllBrand();
                var brandList = brandDataList.BrandCardList;
                var useraddress = brandDataList.AddressList;
                var brand = brandData.Brands;
                var searchzoneList = brandData.SearchZoneList;
                var websiteList = brandData.WebsiteList;

                var brandstoreVM = new BrandStoreViewModel
                {
                    Brands = new BrandStoreViewModel.BrandData
                    {
                        BrandName = brand.BrandName,
                        BrandStar = brand.BrandStar == 0.0 ? "0.0" : (brand.BrandStar / brand.BrandStarAmount).ToString("0.0"),
                        BrandStarAmount = brand.BrandStarAmount,
                        Description = brand.Description,
                        Video = brand.Video,
                    },

                    SearchZoneList = searchzoneList.Select(x => new SearchZoneViewModel
                    {
                        BrandAdImgPath = x.BrandAdImgPath,
                    }),

                    WebsiteList = websiteList.Select(x => new BrandStoreViewModel.WebsitebDataList
                    {
                        PlatformId = x.PlatformId,
                        Path = x.Path,
                        Webpic = x.Webpic,
                        Name = x.Name,
                        MediaLogo = x.MediaLogo,
                    }),
                };
                
                return View(brandstoreVM);
            }
        }


        public IActionResult BrandZone()
        {
            var brandcard = _storeService.GetAllBrand();

            var brandcardVM = new BrandCardViewModel
            {
                BrandCardList = brandcard.BrandCardList.Select(x => new BrandCardViewModel.BrandCardDataList
                {
                    BrandId = x.BrandId,
                    BrandLogo = x.BrandLogo,
                    BrandName = x.BrandName,
                    BrandSlogan = x.BrandSlogan,
                    CardImgUrl = x.CardImgUrl,
                    Srar = x.BrandStar == 0.0 ? "0.0" : (x.BrandStar / x.BrandStarAmount).ToString("0.0"),
                    Categories = x.Categories.Select(y => new BrandCardViewModel.BrandCardDataList.Category { CategoryFig = y.CategoryFig, CategoryName = y.CategoryName }).ToList(),
                })
            };

            return View(brandcardVM);
        }

        public IActionResult ShopMenu(int id)
        {
            if (!_storeService.ShopExist(id))
            {
                TempData["ErrorMessage"] = "此品牌暫不服務，請選擇其他品牌，感謝!";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }

            if (! _storeService.ShopMenuExist(id))
            {
                TempData["ErrorMessage"] = "此店家尚無菜單，敬請期待!";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }
            
            if (! _storeService.IsShopAvaliable(id))
            {
                TempData["ErrorMessage"] = "此店家暫無營業!";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }

            var shop = _storeService.GetShopMenu(id);
            var shopVM = _mapper.Map<StoreShopMenuViewModel>(shop);

            shopVM.Span = "歡迎光臨";
            shopVM.IsOpen = _storeService.IsOpen(id);
            shopVM.ShopId = id;
            //shopVM.MenuClassJson = Extensions.JsonSerialize(shop.MenuClasses);
            shopVM.ProductDetailModal = new ProductDetailModal
            {
                BtnText = "加入購物車",
                BtnEnable = true,
                HasSticker = shopVM.HasSticker
            };

            if (! _storeService.IsOpen(id))
            {
                shopVM.Span = $"非營業時間，" + (shop.PreOrder ? "開放預訂" : "請在營業時間再光臨!");
                shopVM.ProductDetailModal.BtnText = shop.PreOrder ? "預訂" : "無法點餐";
                shopVM.ProductDetailModal.BtnEnable = shop.PreOrder ? true : false;
            }


            if (User.Identity.Name != null)
            {
                var productInCartDto = _orderService.GetAmountInCart(int.Parse(User.Identity.Name), shopVM.ShopId);
                if (productInCartDto != null)
                {
                    shopVM.ProductInCart = _mapper.Map<ProductInCart>(productInCartDto);
                }
            }
            else
            {
                shopVM.ProductDetailModal.BtnText = "登入來點餐";
            }

            shopVM.FirstColor ??= "#0098ff";
            shopVM.SecondColor ??= "#000";

            return View(shopVM);
        }

        [Authorize]
        public IActionResult ShopMenuByGroup(string groupId)
        {
            var cart = _orderService.GetUserAndShopFromGroupId(groupId);

            if (cart == null)
            {
                TempData["ErrorMessage"] = "找不到此團購";
                return RedirectToAction("ErrorMessage", "ErrorHandler");
            }
            
            var shopId = cart.ShopId;
            var leaderId = cart.UserAccountId;
            var currentUserId = int.Parse(User.Identity.Name);

            var shop =  _storeService.GetShopMenu(shopId);
            var shopVM = _mapper.Map<StoreShopMenuViewModel>(shop);

            shopVM.Span = "歡迎光臨";
            shopVM.IsOpen = _storeService.IsOpen(shopId);
            shopVM.ShopId = shopId;
            //shopVM.MenuClassJson = Extensions.JsonSerialize(shop.MenuClasses);
            shopVM.ProductDetailModal = new ProductDetailModal
            {
                BtnText = "加入購物車",
                BtnEnable = true,
                HasSticker = shopVM.HasSticker
            };

            if (!_storeService.IsOpen(shopId))
            {
                shopVM.Span = $"非營業時間，" + (shop.PreOrder ? "開放預訂" : "請在營業時間再光臨!");
                shopVM.ProductDetailModal.BtnText = shop.PreOrder ? "預訂" : "無法點餐";
                shopVM.ProductDetailModal.BtnEnable = shop.PreOrder ? true : false;
            }

            shopVM.LeaderId = leaderId;

            var productInCartDto = _orderService.GetAmountInCart(leaderId, shopVM.ShopId);
            if (productInCartDto != null)
            {
                shopVM.ProductInCart = _mapper.Map<ProductInCart>(productInCartDto);
                shopVM.ProductInCart.IsLeader = true;
                shopVM.ProductInCart.MsgForGroup = "團購中";

                if (currentUserId != leaderId) {
                    shopVM.ProductInCart.IsLeader = false;
                    shopVM.ProductInCart.MsgForGroup = _storeService.MsgForGroupMember(leaderId);
                } 
            }

            return View("ShopMenu", shopVM);
        }

        [HttpGet]
        public IActionResult GetMenuData(int id)
        {
            var menuData = _storeService.GetMenuDetail(id);

            return new JsonResult(Extensions.JsonSerialize(menuData));
        }

        public IActionResult Search(SearchDataModel request)
        {
                var searchDto = _storeService.InitSearchPage();
                var shopCards = _storeService.GetShopCards(request.SearchReDMTransToSearchReDto());
                var searchVM = TransformService.DtoTransToSearchVM(request, searchDto, shopCards);
                return View(searchVM);
        }
        [HttpPost]
        public IActionResult NearbyShopCards([FromBody] SearchDataModel request)
        {
            var shopCards=_storeService.GetNearbyShopCards(request.SearchReDMTransToSearchReDto()).ShopCardDtoTransToShopCardVM();
            return PartialView("_SearchShopCardsPartial", shopCards);
        }

        [HttpPost]
        public IActionResult SearchShops([FromBody] SearchDataModel request)
        {
            var searchShopCards = _storeService.GetShopCards( request.SearchReDMTransToSearchReDto()).ShopCardDtoTransToShopCardVM();
            return PartialView ( "_SearchShopCardsPartial", searchShopCards);
        }
        public IActionResult AddFavorite(int id)
        {
            return Content(_storeService.UpdateFavorite("Add",id));
        }
        public IActionResult RemoveFavorite(int id)
        {
            return Content(_storeService.UpdateFavorite("Remove",id));
        }
        [Route("/Store/SearchBrand/{input?}")]
        public IActionResult SearchBrand(string input)
        {
            var brands=_storeService.GetBrandItem(input);
            return PartialView("_SearchBrandItemPartial",brands);
        }
    }
}
