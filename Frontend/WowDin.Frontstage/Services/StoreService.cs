using CoreMVC_Project.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models.Dto.PartialView;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Models.ViewModel.Store;
using WowDin.Frontstage.Repositories;
using WowDin.Frontstage.Repositories.Interface;
using WowDin.Frontstage.Services.Interface;
using static WowDin.Frontstage.Common.ModelEnum.ShopMethodEnum;

namespace WowDin.Frontstage.Services.Store
{
    public class StoreService : IStoreService
    {
        private readonly IConfiguration _config;
        private readonly IRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        public StoreService(IRepository repository, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration config, IMemoryCacheRepository memoryCacheRepository)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _memoryCacheRepository = memoryCacheRepository;
        }

        public bool BrandExist(int brandId)
        {
            var brand = _repository.GetAll<Brand>().Where(x => x.Verified == 1 && x.Suspension == false);
            var brandCheck = brand.FirstOrDefault(x => x.BrandId == brandId);
            if (brandCheck == null) { return false; }
            return true;
        }
        public bool ShopExist(int shopId)
        {
            var brand = _repository.GetAll<Brand>().Where(x => x.Verified == 1 && x.Suspension == false);
            var shop = _repository.GetAll<Shop>().FirstOrDefault(x => x.ShopId == shopId).BrandId;
            var shopCheck = brand.FirstOrDefault(x => x.BrandId == shop);
            if (shopCheck == null) { return false; }
            return true;
        }

        public bool ShopMenuExist(int shopId)
        {
            var shop = _repository.GetAll<Shop>().FirstOrDefault(x => x.ShopId == shopId);
            if (shop == null) 
                return false; 

            var menuClasses = _repository.GetAll<MenuClass>().Where(x => x.ShopId == shopId);
            if (menuClasses.Count() == 0) 
                return false; 

            var products = _repository.GetAll<Product>().Where(x => menuClasses.Select(c => c.MenuClassId).Contains(x.MenuClassId));
            if (products.Count() == 0) 
                return false; 

            if (GetShopMethod(shopId, TakeMethodEnum.TakeOut).Count() == 0 && GetShopMethod(shopId, TakeMethodEnum.Delivery).Count() == 0)  
                return false; 
            
            return true;
        }
        public ShopMenuDto GetShopMenu(int shopId)
        {
            var shopDto = _memoryCacheRepository.Get<ShopMenuDto>($"Store.GetShopMenu({shopId})");

            var shop = _repository.GetAll<Shop>().First(x => x.ShopId == shopId);
            var brand = _repository.GetAll<Brand>().First(b => b.BrandId == shop.BrandId);

            if(shopDto == null)
            {
                shopDto = new ShopMenuDto()
                {
                    BrandName = brand.Name,
                    BrandLogo = brand.Logo,
                    BrandId = shop.BrandId,
                    Name = shop.Name,
                    MapData = new Map { FullAddress = shop.City + shop.District + shop.Address, Lat = shop.Lat.ToString(), Lng = shop.Lng.ToString() },
                    Star = Convert.ToDouble(shop.Star),
                    StarAmount = shop.StarAmount,
                    Phone = shop.Phone,
                    FullAddress = shop.City + shop.District + shop.Address,
                    PayMethod = _repository.GetAll<ShopPaymentType>().Where(x => x.ShopId == shopId).Select(x => ((ShopPaymentTypeEnum.PaymentTypeEnum)x.PaymentType).GetDescription()).ToList(),
                    PreOrder = shop.PreOrder,
                    PriceLimit = shop.PriceLimit == null ? "單筆消費無上限" : $"單筆消費上限{ ((int)shop.PriceLimit).ToString("C0", CultureInfo.CreateSpecificCulture("en-US")) }",
                    HasSticker = shop.HasSticker,
                    ShopFigures = _repository.GetAll<ShopFigure>().Where(x => x.BrandId == brand.BrandId).OrderBy(x => x.Sort).Select(x => new FigureDto { Path = x.Path, Alt = x.AltText }).ToList(),
                    FirstColor = brand.FirstColor,
                    SecondColor = brand.SecondColor,
                    Promotions = _repository.GetAll<Coupon>().Where(x => x.ShopId == shop.ShopId).Select(x => new PromotionDto { Name = x.CouponName, Description = x.Description }).ToList(),
                    TakeOuts = GetShopMethod(shopId, TakeMethodEnum.TakeOut).ToList(),
                    Deliveries = GetShopMethod(shopId, TakeMethodEnum.Delivery).ToList(),
                    //MenuClasses =  GetMenuDetail(shop.ShopId)
                };

                if (shop.State == (int)ShopEnum.StateEnum.Auto)
                {
                    shopDto.OpenTimeSpan = shop.OpenTime.TransferToTaipeiTime().TimeOfDay.ToString(@"hh\:mm") + "~" + shop.CloseTime.TransferToTaipeiTime().TimeOfDay.ToString(@"hh\:mm");
                }
                else
                {
                    shopDto.OpenTimeSpan = "";
                }

                shopDto.FirstColor ??= "#0098ff";
                shopDto.SecondColor ??= "#000";

                _memoryCacheRepository.Set($"Store.GetShopMenu({shopId})", shopDto);
            }

            return shopDto;
        }

        public bool IsShopAvaliable(int shopId)
        {
            var shop = _repository.GetAll<Shop>().First(x => x.ShopId == shopId);

            return shop.State == (int)ShopEnum.StateEnum.Auto ||
                    shop.State == (int)ShopEnum.StateEnum.Open ||
                    shop.State == (int)ShopEnum.StateEnum.Rest;
        }

        public bool IsOpen(int shopId)
        {
            //OpenTime與CloseTime預設2000/01/01
            var shop = _repository.GetAll<Shop>().First(x => x.ShopId == shopId);

            if (shop.State == (int)ShopEnum.StateEnum.Auto)
            {
                var now = DateTime.Now.TransferToTaipeiTime();
                var openDaysList = shop.OpenDayList.Split(',');
                if (!openDaysList.Contains(now.DayOfWeek.ToString()))
                {
                    return false;
                }

                var openTime = shop.OpenTime.TransferToTaipeiTime();
                var closeTime = shop.CloseTime.TransferToTaipeiTime();
                //現在時間早於開店時間或晚於關店時間
                if (now.TimeOfDay.CompareTo(openTime.TimeOfDay) < 0 || now.TimeOfDay.CompareTo(closeTime.TimeOfDay) > 0)
                {
                    return false;
                }

                return true;
            }

            if (shop.State == (int)ShopEnum.StateEnum.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerable<MethodDto> GetShopMethod(int shopId, TakeMethodEnum type)
        {
            var shop = _repository.GetAll<Shop>().First(x => x.ShopId == shopId);
            var shopMethods = _repository.GetAll<ShopMethod>().Where(x => x.ShopId == shop.ShopId);

            if (type == TakeMethodEnum.TakeOut)
            {
                var takeOutForShop = shopMethods.FirstOrDefault(x => x.TakeMethod == (int)TakeMethodEnum.TakeOut);
                if (takeOutForShop == null) return Enumerable.Empty<MethodDto>();
                var takeOutDetails = _repository.GetAll<Takeout>().Where(x => x.ShopMethodId == takeOutForShop.ShopMethodId).Select(x => new MethodDto
                {
                    Condition = x.Condition,
                    Result = $"等待時間大約為{x.WaitingTime.TotalMinutes}分鐘"
                });
                return takeOutDetails;
            }

            else
            {
                var deliveryForShop = shopMethods.FirstOrDefault(x => x.TakeMethod == (int)TakeMethodEnum.Delivery);
                if (deliveryForShop == null) return Enumerable.Empty<MethodDto>();
                var deliveryDetails = _repository.GetAll<Delivery>().Where(x => x.ShopMethodId == deliveryForShop.ShopMethodId).Select(x => new MethodDto
                {
                    Condition = $"滿${x.PriceThreshold}",
                    Result = $"可外送{x.LowerDistance}公里至{x.HigherDistance}公里"
                });
                return deliveryDetails;
            }
        }

        public string MsgForGroupMember(int leaderId)
        {
            var leaderName = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == leaderId).RealName.Trim();
            return $"正在加入 {leaderName} 的團購";
        }

        public IEnumerable<ProductClassDto> GetMenuDetail(int shopId)
        {
            var menuResult = _memoryCacheRepository.Get<List<ProductClassDto>>($"Store.GetMenuDetail({shopId})");

            if(menuResult == null)
            {
                var menuList = _repository.GetAll<MenuClass>().Where(x => x.ShopId == shopId).ToList().OrderBy(x => x.Sort);
                var productList = _repository.GetAll<Product>().Where(x => menuList.Select(y => y.MenuClassId).Contains(x.MenuClassId) && 
                                    x.State != (int)ProductEnum.StateEnum.Deleted && x.State != (int)ProductEnum.StateEnum.Disable).ToList().OrderBy(x => x.Sort);
                var customList = _repository.GetAll<Custom>().Where(x => productList.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
                var selectionList = _repository.GetAll<CustomSelection>().Where(x => customList.Select(y => y.CustomId).Contains(x.CustomId)).ToList();

                menuResult = menuList.Select(c => new ProductClassDto
                {
                    Name = c.ClassName,
                    Products = productList.Where(p => p.MenuClassId == c.MenuClassId).GetProductsWithCustom(customList, selectionList)
                }).ToList();

                _memoryCacheRepository.Set($"Store.GetMenuDetail({shopId})", menuResult);
            }

            return menuResult;
        }

        public GetAllBrandDto GetAllBrand()
        {
            var brandCheck = _repository.GetAll<Brand>().Where(x => x.Verified == 1 && x.Suspension == false);
            var brandData = _repository.GetAll<Brand>().Where(x => brandCheck.Select(b => b.BrandId).Contains(x.BrandId)).ToList();
            var shopData = _repository.GetAll<Shop>();
            var commentData = _repository.GetAll<Comment>();
            var catagorySetsData = _repository.GetAll<CatagorySet>();
            var catagoryData = _repository.GetAll<Catagory>();
            var addressData = _repository.GetAll<AddressOption>();

            var allbrandDto = new GetAllBrandDto()
            {
                BrandCardList = brandData.Select(x => new BrandCardDto
                {
                    BrandId = x.BrandId,
                    BrandLogo = x.Logo,
                    BrandName = x.Name,
                    BrandSlogan = x.Slogen,
                    CardImgUrl = x.CardImgUrl,
                    BrandStar = Convert.ToDouble(x.Star),
                    BrandStarAmount = x.StarAmount,
                    Categories = catagorySetsData.Where(y => y.BrandId == x.BrandId).Select(y => new BrandCardDto.Category
                    {
                        CategoryFig = catagoryData.First(z => z.CatagoryId == y.CatagoryId).Fig,
                        CategoryName = catagoryData.First(z => z.CatagoryId == y.CatagoryId).Name
                    }).ToList(),
                }),

                AddressList = addressData.Select(x => new AddressDto
                {
                    UserAccountId = x.UserAccountId,
                    Name = x.Name,
                    City = x.City,
                    District = x.District,
                    Address = x.Address,
                })
            };

            return allbrandDto;
        }

        public GetBrandDataDto GetBrandDataById(int id)
        {
            var brandData = _repository.GetAll<Brand>().FirstOrDefault(x => x.BrandId == id);
            var brandListData = _repository.GetAll<Brand>().Where(x => x.BrandId == id).ToList();
            var webDataList = _repository.GetAll<Website>().Where(x => x.BrandId == id).ToList();
            var brandfigDataList = _repository.GetAll<BrandFigure>().Where(x => x.BrandId == id);

            var brand = new BrandData
            {
                BrandName = brandData.Name,
                BrandStar = Convert.ToDouble(brandData.Star),
                BrandStarAmount = brandData.StarAmount,
                Description = brandData.Description,
                Video = brandData.Video,
            };

            var brandfigureList = brandfigDataList.Select(x => new SearchZoneDto
            {
                BrandAdImgPath = x.Path,
            });

            var websiteList = webDataList.Select(x => new WebsitebDataList
            {
                PlatformId = x.PlatformId,
                Path = x.Path,
                Webpic = x.Webpic,
                Name = _repository.GetAll<Platform>().FirstOrDefault(y => y.PlatformId == x.PlatformId).Name,
                MediaLogo = _repository.GetAll<Platform>().FirstOrDefault(y => y.PlatformId == x.PlatformId).Logo,
            });

            var brandbyIdDto = new GetBrandDataDto
            {
                Brands = brand,
                SearchZoneList = brandfigureList,
                WebsiteList = websiteList,
            };

            return brandbyIdDto;
        }

        public SearchDto InitSearchPage()
        {
            var searchDto = _memoryCacheRepository.Get<SearchDto>("Store.InitSearchPage");
            if (searchDto == null)
            {
                var shops = _repository.GetAll<Shop>().ToList();
                var showBrands = shops.Where(x => ShopMenuExist(x.ShopId) && IsShopAvaliable(x.ShopId) && ShopExist(x.ShopId)).Select(x => x.BrandId).Distinct();
                var brands = _repository.GetAll<Brand>().Where(x => showBrands.Contains(x.BrandId)).Select(x => x.Name).ToList();
                var showCategories = _repository.GetAll<CatagorySet>().Where(x => showBrands.Contains(x.BrandId)).Select(x => x.CatagoryId).Distinct();
                var categories = _repository.GetAll<Catagory>().Where(x=>showCategories.Contains(x.CatagoryId));
                searchDto = new SearchDto()
                {
                    Brands = brands,
                    Categories = categories.Select(x => new SearchDto.Category { Name = x.Name, Img = x.Fig }).ToList()
                };
                _memoryCacheRepository.Set("Store.InitSearchPage", searchDto);
            }
            return searchDto;
        }
        public IEnumerable<ShopCardDto> GetNearbyShopCards(SearchRequestDto request)
        {
            var shopCards=GetShopCards(request);
            if (shopCards.Count() > 6)
            {
                shopCards = shopCards.Take(6);
            }
            return shopCards;
        }
        public IEnumerable<ShopCardDto> GetShopCards(SearchRequestDto request)
        {
            if (!request.IsValid()) return new List<ShopCardDto>();
            var shopCards = _memoryCacheRepository.Get<List<ShopCardDto>>("Store.GetShopCards");
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
            if (shopCards == null)
            {
                var shops = _repository.GetAll<Shop>().ToList();
                shops = shops.Where(x => ShopMenuExist(x.ShopId) && IsShopAvaliable(x.ShopId) && ShopExist(x.ShopId)).ToList();
                if (shops.Count() > 0)
                {
                    var brands = _repository.GetAll<Brand>();
                    var categories = _repository.GetAll<Catagory>();
                    var catagorySets = _repository.GetAll<CatagorySet>();
                    var shopMethods = _repository.GetAll<ShopMethod>();
                    shopCards = shops.Select(x => new ShopCardDto()
                    {
                        ShopId = x.ShopId,
                        ShopFig = brands.First(y => y.BrandId == x.BrandId).CardImgUrl,
                        ShopName = x.Name,
                        BrandId = x.BrandId,
                        BrandLogo = brands.First(y => y.BrandId == x.BrandId).Logo,
                        BrandName = brands.First(y => y.BrandId == x.BrandId).Name,
                        OpenTime = x.OpenTime.TransferToTaipeiTime().ToString("HH:mm"),
                        CloseTime = x.CloseTime.TransferToTaipeiTime().ToString("HH:mm"),
                        Star = x.Star == 0.0 ? 0.0 : Convert.ToDouble(x.Star) / x.StarAmount,
                        Address = x.City + x.District + x.Address,
                        Lat=x.Lat,
                        Lng=x.Lng,
                        IsDelivery = shopMethods.Where(y => y.ShopId == x.ShopId).Any(y => y.TakeMethod == (int)TakeMethodEnum.Delivery),
                        Categories = catagorySets.Where(y => y.BrandId == x.BrandId).Select(y => new ShopCardDto.Category
                        {
                            CategoryId = y.CatagoryId,
                            CategoryFig = categories.First(z => z.CatagoryId == y.CatagoryId).Fig,
                            CategoryName = categories.First(z => z.CatagoryId == y.CatagoryId).Name
                        }).ToList()
                    }).ToList();
                    _memoryCacheRepository.Set("Store.GetShopCards", shopCards);
                }
                else
                {
                    _memoryCacheRepository.Set("Store.GetShopCards", new List<ShopCardDto>());
                    return new List<ShopCardDto>();
                }
            }

            var orders = _repository.GetAll<Models.Entity.Order>();

            switch (request.Method.ToLower())
            {
                case "all":
                    break;

                case "nearby":
                    shopCards = request.Lat == 0 && request.Lng == 0 ? null : shopCards;
                    break;

                case "area":
                    shopCards = string.IsNullOrEmpty(request.Address) || string.IsNullOrWhiteSpace(request.Address) ? null : shopCards.Where(x => x.Address.Contains(request.Address)).ToList();
                    break;

                case "ordered":
                    shopCards = userIdentity.IsAuthenticated ? shopCards.Where(x => orders.Where(y => y.UserAcountId == int.Parse(userIdentity.Name)).Select(y => y.ShopId).Contains(x.ShopId)).ToList() : shopCards.Take(0).ToList();
                    break;

            }
            if (shopCards != null && shopCards.Count() > 0)
            {
                if (request.Brand != "全部" && !string.IsNullOrEmpty(request.Brand) && !string.IsNullOrWhiteSpace(request.Brand))
                {
                    shopCards = shopCards.Where(x => x.BrandName == request.Brand).ToList();
                }
                if (request.Category != "全部" && !string.IsNullOrEmpty(request.Category) && !string.IsNullOrWhiteSpace(request.Category))
                {
                    shopCards = shopCards.Where(x => x.Categories.Select(y => y.CategoryName).Contains(request.Category)).ToList();
                }
                if (request.Evaluate != "全部" && !string.IsNullOrEmpty(request.Evaluate) && !string.IsNullOrWhiteSpace(request.Evaluate))
                {
                    var evaluate = request.Evaluate.Trim();
                    shopCards = shopCards.Where(x => x.Star >= double.Parse(evaluate.Length > 1 ? evaluate.Remove(1) : evaluate)).ToList();
                }
            }
            if (shopCards != null && shopCards.Count() > 0)
            {
                var favorites = _repository.GetAll<Favorite>();
                foreach (var shop in shopCards)
                {
                    shop.Distance = request.Lat != 0 && request.Lng != 0 ? new GeoCoordinate(shop.Lat, shop.Lng).GetDistanceTo(new GeoCoordinate(request.Lat, request.Lng)) / 1000 : 0;
                    shop.IsFavorite = userIdentity.IsAuthenticated ? favorites.Where(y => y.UserAccountId == int.Parse(userIdentity.Name)).Select(y => y.ShopId).Contains(shop.ShopId) : false;
                }
                return shopCards.AsEnumerable().OrderBy(x => x.Distance);
            }
            else { return shopCards; }
        }


        public string UpdateFavorite(string method, int shopId)
        {
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
            try
            {
                if (!userIdentity.IsAuthenticated) { return "請先登入會員"; }
                if (method == "Add")
                {
                    var favorite = new Favorite
                    {
                        UserAccountId = int.Parse(userIdentity.Name),
                        ShopId = shopId
                    };
                    _repository.Create(favorite);
                    _repository.Save();
                    return "收藏店家成功";
                }
                else if (method == "Remove")
                {
                    var favorite = _repository.GetAll<Favorite>().First(x => x.ShopId == shopId && x.UserAccountId == int.Parse(userIdentity.Name));
                    _repository.Delete(favorite);
                    _repository.Save();
                    return "移除收藏成功";
                }
                else
                {
                    return "此方法不存在";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<string> GetBrandItem(string input)
        {
            var brands = _memoryCacheRepository.Get<List<string>>("Store.GetBrandItem");
            if (brands == null)
            {
                var shops = _repository.GetAll<Shop>().ToList();
                var showBrands = shops.Where(x => ShopMenuExist(x.ShopId) && IsShopAvaliable(x.ShopId) && ShopExist(x.ShopId)).Select(x=>x.BrandId).Distinct();
                brands = _repository.GetAll<Brand>().Where(x=>showBrands.Contains(x.BrandId)).Select(x => x.Name).ToList();
                _memoryCacheRepository.Set("Store.GetBrandItem", brands);
            }
            return string.IsNullOrEmpty(input) ? brands : brands.Where(x => x.ToLower().Contains(input.Replace(" ", "").ToLower())).ToList();
        }
        public DeliveryFeeDto GetDevliveryFee(int shopId, string address, decimal amount)
        {
            var userPosition = GetUserPosition(address).Result;
            var shop = _repository._context.Find<Shop>(shopId);
            if (_repository.GetAll<ShopMethod>().Any(x => x.ShopId == shopId && x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery))
            {
                var shopMethodId = _repository.GetAll<ShopMethod>().First(x => x.ShopId == shopId && x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery).ShopMethodId;
                var deliverys = _repository.GetAll<Delivery>().Where(x => x.ShopMethodId == shopMethodId);
                var distance = double.Parse((new GeoCoordinate(userPosition.Lat, userPosition.Lng).GetDistanceTo(new GeoCoordinate(shop.Lat, shop.Lng)) / 1000).ToString("0.0"));
                if (deliverys.Any(x => amount >= x.PriceThreshold && distance >= x.LowerDistance && distance <= (x.HigherDistance ?? double.MaxValue)))
                {
                    return new DeliveryFeeDto
                    {
                        IsConform = true,
                        Message = "符合外送條件",
                        DeliveryFee = deliverys.OrderBy(x => x.DeliveryFee).First(x => amount >= x.PriceThreshold && distance >= x.LowerDistance && distance <= (x.HigherDistance ?? double.MaxValue)).DeliveryFee ?? 0
                    };
                }
                else if (deliverys.Any(x => distance >= x.LowerDistance && distance <= (x.HigherDistance ?? double.MaxValue)))
                {
                    var delivery = deliverys.OrderBy(x => x.PriceThreshold).First(x => distance >= x.LowerDistance && distance <= (x.HigherDistance ?? double.MaxValue));
                    var threshold = delivery.PriceThreshold ?? 0;
                    var fee = delivery.DeliveryFee ?? 0;
                    var diff = threshold - amount;
                    return new DeliveryFeeDto
                    {
                        IsConform = false,
                        Message = $"消費金額未達外送門檻 還差{diff.ToString("0.#")}元",
                        DeliveryFee = fee
                    };
                }
                else
                {
                    return new DeliveryFeeDto { IsConform = false, Message = "距離過遠 店家無法提供外送服務", DeliveryFee = null };
                }

            }
            else
            {
                return new DeliveryFeeDto { IsConform = false, Message = "此店家未提供外送服務", DeliveryFee = null };
            }
        }
        private async Task<UserPositionDto> GetUserPosition(string address)
        {
            var api = _config["GoogleMap:Api"];
            var key = _config["GoogleMap:Key"];
            var requestUri = @$"{api}address={address}&key={key}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Accept", "application/json");
            var client = _httpClientFactory.CreateClient();
            var response = client.Send(request);
            var position = JsonSerializer.DeserializeAsync<PositionDto>(await response.Content.ReadAsStreamAsync()).Result;
            var userPosition = new UserPositionDto { Lat = (double)position.results[0].geometry.location.lat, Lng = (double)position.results[0].geometry.location.lng };
            return userPosition;
        }

        public StoreShopMenuViewModel GetShopMenuBasicVM(int shopId)
        {
            throw new NotImplementedException();
        }
    }

    public static class StoreExtension
    {
        //效能調校前
        public static IEnumerable<ProductDto> GetProductsWithCustom(this IEnumerable<Product> products, IRepository repository)
        {
            return products.Select(p => new ProductDto
            {
                Name = p.Name,
                ProductId = p.ProductId,
                Figure = p.Fig,
                BasicPrice = p.BasicPrice,
                SellOut = (ProductEnum.StateEnum)p.State == ProductEnum.StateEnum.Disable ? true : false,
                Customs = repository.GetAll<Custom>().Where(cu => cu.ProductId == p.ProductId).AsNoTracking().ToList().Select(cu => new CustomDto
                {
                    Name = cu.Name,
                    MaxAmount = cu.MaxAmount,
                    Necessary = cu.Necessary,
                    Selections = repository.GetAll<CustomSelection>().Where(s => s.CustomId == cu.CustomId).AsNoTracking().ToList().Select(s => new SelectionDto
                    {
                        Name = s.Name,
                        IsSelected = false,
                        AddPrice = s.AddPrice
                    })
                })
            });
        }

        //效能調校後
        public static IEnumerable<ProductDto> GetProductsWithCustom(this IEnumerable<Product> products, IEnumerable<Custom> customs, IEnumerable<CustomSelection> customSelections)
        {
            return products.Select(p => new ProductDto
            {
                Name = p.Name,
                ProductId = p.ProductId,
                Figure = p.Fig,
                BasicPrice = p.BasicPrice,
                SellOut = (ProductEnum.StateEnum)p.State == ProductEnum.StateEnum.Disable ? true : false,
                Customs = customs.Where(cu => cu.ProductId == p.ProductId).ToList().Select(cu => new CustomDto
                {
                    Name = cu.Name,
                    MaxAmount = cu.MaxAmount,
                    Necessary = cu.Necessary,
                    Selections = customSelections.Where(s => s.CustomId == cu.CustomId).ToList().Select(s => new SelectionDto
                    {
                        Name = s.Name,
                        IsSelected = false,
                        AddPrice = s.AddPrice
                    })
                })
            });
        }

        public static bool IsValid(this SearchRequestDto request)
        {
            var method = new List<string> { "all", "nearby", "area", "ordered" }.Contains(request.Method.ToLower());
            var position = request.Lat >= -90 && request.Lat <= 90 && request.Lng >= -180 && request.Lng <= 180;
            var address = string.IsNullOrEmpty(request.Address) || string.IsNullOrWhiteSpace(request.Address) ? true : Regex.IsMatch(request.Address.Trim(), @"^[\u4e00-\u9fa50-9]*$");
            var brand = string.IsNullOrEmpty(request.Brand) || string.IsNullOrWhiteSpace(request.Brand) ? true : Regex.IsMatch(request.Brand.Trim(), @"^[\u4e00-\u9fa50-9a-zA-Z]*$");
            var category = string.IsNullOrEmpty(request.Category) || string.IsNullOrWhiteSpace(request.Category) ? true : Regex.IsMatch(request.Category.Trim(), @"^[\u4e00-\u9fa5]*$");
            var evaluate = string.IsNullOrEmpty(request.Evaluate) || string.IsNullOrWhiteSpace(request.Evaluate) || request.Evaluate == "全部" ? true : Regex.IsMatch(request.Evaluate.Trim().Length > 1 ? request.Evaluate.Trim().Remove(1) : request.Evaluate.Trim(), @"^[0-5]{1}$");
            if (method && position && address && brand && category && evaluate) { return true; }
            else { return false; }
        }

    }
}
