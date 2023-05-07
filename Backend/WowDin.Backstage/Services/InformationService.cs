using Imgur.API.Endpoints;
using Imgur.API.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Models.Dto.Information;
using System;
using System.Collections.Generic;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Models.Entity;
using System.Linq;
using WowDin.Backstage.Common;
using WowDin.Backstage.Common.ModelEnum;
using System.Text.Json;

namespace WowDin.Backstage.Services
{
    public class InformationService : IInformationService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClient;
        private readonly IRepository _repository;
        private readonly int _userId;
        public InformationService(IConfiguration config, IHttpClientFactory httpClient, IRepository repository, IHttpContextAccessor contextAccessor)
        {
            _config = config;
            _httpClient = httpClient;
            _repository = repository;
            _userId = int.Parse(contextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<UploadImageDto> UploadImage(IFormFile file)
        {
            var fileType = new List<string> { "image/jpeg", "image/png", "image/jpg" };
            if (file == null || file.Length == 0)
            {
                return new UploadImageDto { IsSuccess = false, Message = "未選擇檔案 上傳失敗" };
            }
            if (!fileType.Contains(file.ContentType))
            {
                return new UploadImageDto { FileName = file.FileName, IsSuccess = false, Message = "檔案類型不合規 上傳失敗" };
            }
            try
            {
                var img = file.OpenReadStream();
                var clientId = _config["Imgur:ClientId"];
                var clientSecret = _config["Imgur:ClientSecret"];
                var ApiClient = new ApiClient(clientId, clientSecret);
                var httpClient = _httpClient.CreateClient();
                var endpoint = new ImageEndpoint(ApiClient, httpClient);
                var imageUpload = await endpoint.UploadImageAsync(img);
                return new UploadImageDto { FileName = file.FileName, FileUrl = imageUpload.Link, IsSuccess = true, Message = "上傳成功" };
            }
            catch (Exception ex)
            {
                return new UploadImageDto { FileName = file.FileName, IsSuccess = false, Message = $"{ex.Message} 上傳失敗" };
            }
        }
        public async Task<List<UploadImageDto>> UploadImages(List<IFormFile> files)
        {
            var Images = new List<UploadImageDto>();
            if (files == null || files.Count == 0)
            {
                Images.Add(new UploadImageDto { IsSuccess = false, Message = "未選擇檔案 上傳失敗" });
            }
            else
            {
                foreach (var file in files)
                {
                    Images.Add(await UploadImage(file));
                }
            }
            return Images;
        }
        public BrandManagementDto BrandManagementInit()
        {
            var brand = _repository.GetAll<Brand>().First(x => x.BrandId == _userId);
            var brandManagement = new BrandManagementDto
            {
                Star = brand.Star == 0 ? "0.0" : (brand.Star / brand.StarAmount).ToString("0.0"),
                Categories = _repository.GetAll<Catagory>().Select(x => new BrandManagementDto.Category
                {
                    Name = x.Name,
                    Id = x.CatagoryId,
                    Image = x.Fig
                })
            };
            return brandManagement;
        }
        public BrandFacadeDto GetBrandFacade()
        {
            var brand = _repository.GetAll<Brand>().First(x => x.BrandId == _userId);
            var categories = _repository.GetAll<Catagory>();
            var history = _repository.GetAll<BrandHistory>().Where(x => x.BrandId == _userId && x.UpdateTitle.Contains("BrandFacade"));
            return new BrandFacadeDto
            {
                UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                Name = brand.Name,
                CardImgUrl = brand.CardImgUrl,
                Slogan = brand.Slogen,
                Logo = brand.Logo,
                BrandCategories = categories.Where(x => _repository.GetAll<CatagorySet>().Where(y => y.BrandId == _userId).Select(y => y.CatagoryId).Contains(x.CatagoryId)).Select(x => new BrandManagementDto.Category
                {
                    Name = x.Name,
                    Id = x.CatagoryId,
                    Image = x.Fig
                }),
            };

        }
        public BrandIntroduceDto GetBrandIntroduce()
        {
            var brand = _repository.GetAll<Brand>().First(x => x.BrandId == _userId);
            var history = _repository.GetAll<BrandHistory>().Where(x => x.BrandId == _userId && x.UpdateTitle.Contains("BrandIntroduce"));
            return new BrandIntroduceDto
            {
                UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                Description = brand.Description,
                Video = brand.Video,
                FirstColor = brand.FirstColor==null?null:brand.FirstColor.Trim(),
                SecondColor = brand.SecondColor ==null? null:brand.SecondColor.Trim()
            };
        }
        public BrandWebDto GetBrandWeb()
        {
            var websites = _repository.GetAll<Website>().Where(x => x.BrandId == _userId);
            var history = _repository.GetAll<BrandHistory>().Where(x => x.BrandId == _userId && x.UpdateTitle.Contains("BrandWeb"));
            return new BrandWebDto
            {
                UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                FaceBook = websites.Any(x => x.PlatformId == 2) ? websites.First(x => x.PlatformId == 2).Path : "",
                Instagram = websites.Any(x => x.PlatformId == 3) ? websites.First(x => x.PlatformId == 3).Path : "",
                Line = websites.Any(x => x.PlatformId == 4) ? websites.First(x => x.PlatformId == 4).Path : "",
                Official = websites.Any(x => x.PlatformId == 1) ? websites.First(x => x.PlatformId == 1).Path : "",
                OfficialImage = websites.Any(x => x.PlatformId == 1) ? websites.First(x => x.PlatformId == 1).Webpic : "",
            };
        }
        public BrandImagesDto GetBrandImages()
        {
            var history = _repository.GetAll<BrandHistory>().Where(x => x.BrandId == _userId && x.UpdateTitle.Contains("BrandImages"));
            return new BrandImagesDto
            {
                UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                Images = _repository.GetAll<BrandFigure>().Where(x => x.BrandId == _userId).Select(x => new BrandImagesDto.Image { Id = x.BrandFigureId, ImageUrl = x.Path, ImageAlt = x.AltText.Trim(), Sort = x.Sort ?? 0, /*TurnHome = x.Url == "index" ? true : false*/ }).OrderBy(x => x.Sort)
            };
        }
        public ShopImagesDto GetShopImages()
        {
            var history = _repository.GetAll<BrandHistory>().Where(x => x.BrandId == _userId && x.UpdateTitle.Contains("ShopImages"));
            return new ShopImagesDto
            {
                UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                Images = _repository.GetAll<ShopFigure>().Where(x => x.BrandId == _userId).Select(x => new ShopImagesDto.Image { Id = x.ShopFigureId, ImageUrl = x.Path, ImageAlt = x.AltText.Trim(), Sort = x.Sort ?? 0 }).OrderBy(x => x.Sort)
            };
        }
        public SaveDto SaveBrandFacade(BrandFacadeDto brandFacade)
        {
            if (brandFacade == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                var brand = _repository.GetAll<Brand>().First(x => x.BrandId == _userId);
                var categories = _repository.GetAll<CatagorySet>().Where(x => x.BrandId == _userId).ToList();
                brand.CardImgUrl = brandFacade.CardImgUrl;
                brand.Logo = brandFacade.Logo;
                brand.Slogen = brandFacade.Slogan;
                _repository.Update(brand);
                var removeCategories = categories.Select(x => x.CatagoryId).Except(brandFacade.BrandCategories.Select(x => x.Id));
                if (removeCategories.Count() > 0)
                {
                    foreach (var c in categories.Where(x => removeCategories.Contains(x.CatagoryId)))
                    {
                        _repository.Delete(c);
                    }
                }
                var addCategories = brandFacade.BrandCategories.Select(x => x.Id).Except(categories.Select(x => x.CatagoryId));
                if (addCategories.Count() > 0)
                {
                    foreach (var c in addCategories)
                    {
                        _repository.Create(new CatagorySet { CatagoryId = c, BrandId = _userId });
                    }
                }
                var brandHistory = new BrandHistory { BrandId = _userId, UpdateDate = DateTime.UtcNow, UpdateTitle = "BrandFacade", UpdateContent = brandFacade.JsonSerialize() };
                _repository.Create(brandHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = brandHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public SaveDto SaveBrandIntroduce(BrandIntroduceDto brandIntroduce)
        {
            if (brandIntroduce == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                var brand = _repository.GetAll<Brand>().First(x => x.BrandId == _userId);
                brand.Video = brandIntroduce.Video;
                brand.Description = brandIntroduce.Description;
                brand.FirstColor = brandIntroduce.FirstColor;
                brand.SecondColor = brandIntroduce.SecondColor;
                _repository.Update(brand);
                var brandHistory = new BrandHistory { BrandId = _userId, UpdateDate = DateTime.UtcNow, UpdateTitle = "BrandIntroduce", UpdateContent = brandIntroduce.JsonSerialize() };
                _repository.Create(brandHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = brandHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }

        }
        public SaveDto SaveBrandWeb(BrandWebDto brandWeb)
        {
            if (brandWeb == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                UpdateWebsite(brandWeb.Official, 1, brandWeb.OfficialImage);
                UpdateWebsite(brandWeb.FaceBook, 2, null);
                UpdateWebsite(brandWeb.Instagram, 3, null);
                UpdateWebsite(brandWeb.Line, 4, null);
                var brandHistory = new BrandHistory { BrandId = _userId, UpdateDate = DateTime.UtcNow, UpdateTitle = "BrandWeb", UpdateContent = brandWeb.JsonSerialize() };
                _repository.Create(brandHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = brandHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        private void UpdateWebsite(string webPath, int webId, string webPic)
        {
            var websites = _repository.GetAll<Website>().Where(x => x.BrandId == _userId);
            if (string.IsNullOrEmpty(webPath.Trim()))
            {
                if (websites.Any(x => x.PlatformId == webId))
                {
                    var web = websites.First(x => x.PlatformId == webId);
                    _repository.Delete(web);
                }
            }
            else
            {
                if (websites.Any(x => x.PlatformId == webId))
                {
                    var web = websites.First(x => x.PlatformId == webId);
                    web.Path = webPath;
                    web.Webpic = webPic;
                    _repository.Update(web);
                }
                else
                {
                    _repository.Create(new Website { BrandId = _userId, PlatformId = webId, Path = webPath, Webpic = webPic });
                }
            }
        }
        public SaveDto SaveBrandImages(BrandImagesDto brandImages)
        {
            if (brandImages == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                var images = _repository.GetAll<BrandFigure>().Where(x => x.BrandId == _userId).ToList();
                var removeImages = images.Select(x => x.BrandFigureId).Except(brandImages.Images.Select(x => x.Id));
                if (removeImages.Count() > 0)
                {
                    foreach (var i in images.Where(x => removeImages.Contains(x.BrandFigureId)))
                    {
                        _repository.Delete(i);
                    }
                }
                var addImages = brandImages.Images.Where(x => x.Id == 0);
                if (addImages.Count() > 0)
                {
                    foreach (var i in addImages)
                    {
                        _repository.Create(new BrandFigure { BrandId = _userId, Path = i.ImageUrl, AltText = i.ImageAlt, Sort = i.Sort,/*Url=i.TurnHome?"index": "orgin"*/ });
                    }
                }
                var updateImages = images.Select(x => x.BrandFigureId).Intersect(brandImages.Images.Select(x => x.Id));
                if (updateImages.Count() > 0)
                {
                    foreach (var i in images.Where(x => updateImages.Contains(x.BrandFigureId)))
                    {
                        var brandImg = brandImages.Images.First(x => x.Id == i.BrandFigureId);
                        i.Path = brandImg.ImageUrl;
                        i.AltText = brandImg.ImageAlt;
                        i.Sort = brandImg.Sort;
                        //i.Url = brandImg.TurnHome ? "index" : "orgin";
                        _repository.Update(i);
                    }
                }
                var brandHistory = new BrandHistory { BrandId = _userId, UpdateDate = DateTime.UtcNow, UpdateTitle = "BrandImages", UpdateContent = brandImages.JsonSerialize() };
                _repository.Create(brandHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = brandHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public SaveDto SaveShopImages(ShopImagesDto shopImages)
        {
            if (shopImages == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                var images = _repository.GetAll<ShopFigure>().Where(x => x.BrandId == _userId).ToList();
                var removeImages = images.Select(x => x.ShopFigureId).Except(shopImages.Images.Select(x => x.Id));
                if (removeImages.Count() > 0)
                {
                    foreach (var i in images.Where(x => removeImages.Contains(x.ShopFigureId)))
                    {
                        _repository.Delete(i);
                    }
                }
                var addImages = shopImages.Images.Where(x => x.Id == 0);
                if (addImages.Count() > 0)
                {
                    foreach (var i in addImages)
                    {
                        _repository.Create(new ShopFigure { BrandId = _userId, Path = i.ImageUrl, AltText = i.ImageAlt, Sort = i.Sort });
                    }
                }
                var updateImages = images.Select(x => x.ShopFigureId).Intersect(shopImages.Images.Select(x => x.Id));
                if (updateImages.Count() > 0)
                {
                    foreach (var i in images.Where(x => updateImages.Contains(x.ShopFigureId)))
                    {
                        var shopImg = shopImages.Images.First(x => x.Id == i.ShopFigureId);
                        i.Path = shopImg.ImageUrl;
                        i.AltText = shopImg.ImageAlt;
                        i.Sort = shopImg.Sort;
                        _repository.Update(i);
                    }
                }
                var brandHistory = new BrandHistory { BrandId = _userId, UpdateDate = DateTime.UtcNow, UpdateTitle = "ShopImages", UpdateContent = shopImages.JsonSerialize() };
                _repository.Create(brandHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = brandHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public ShopManagementDto ShopManagementInit()
        {
            return new ShopManagementDto
            {
                BrandName = _repository.GetAll<Brand>().First(x => x.BrandId == _userId).Name,
                Shops = _repository.GetAll<Shop>().Where(x => x.BrandId == _userId && x.State != (int)ShopEnum.StateEnum.Remove).Select(x => new ShopManagementDto.Shop { Id = x.ShopId, Name = x.Name }),
                States = new List<ShopManagementDto.State> {
                    new(){Id=ShopEnum.StateEnum.Open,Name=ShopEnum.StateEnum.Open.GetDescription()},
                    new(){Id=ShopEnum.StateEnum.Rest,Name=ShopEnum.StateEnum.Rest.GetDescription()},
                    new(){Id=ShopEnum.StateEnum.Close,Name=ShopEnum.StateEnum.Close.GetDescription()},
                },
                DayList = new List<ShopManagementDto.Day> {
                    new(){Value=ShopEnum.DayListEnum.Monday.ToString(),Text=ShopEnum.DayListEnum.Monday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Tuesday.ToString(),Text=ShopEnum.DayListEnum.Tuesday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Wednesday.ToString(),Text=ShopEnum.DayListEnum.Wednesday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Thursday.ToString(),Text=ShopEnum.DayListEnum.Thursday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Friday.ToString(),Text=ShopEnum.DayListEnum.Friday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Saturday.ToString(),Text=ShopEnum.DayListEnum.Saturday.GetDescription()},
                    new(){Value=ShopEnum.DayListEnum.Sunday.ToString(),Text=ShopEnum.DayListEnum.Sunday.GetDescription()},
                },
                CityDistrict = System.IO.File.ReadAllText(@"wwwroot/json/CityDistrict.json").JsonDeserialize<IEnumerable<CityDistrictDto>>(),
                PaymentTypes = new List<ShopManagementDto.PaymentType> {
                    new() { Id=ShopPaymentTypeEnum.PaymentTypeEnum.Cash,Name=ShopPaymentTypeEnum.PaymentTypeEnum.Cash.GetDescription()},
                    //new() { Id=ShopPaymentTypeEnum.PaymentTypeEnum.LinePay,Name=ShopPaymentTypeEnum.PaymentTypeEnum.LinePay.GetDescription()},
                    //new() { Id=ShopPaymentTypeEnum.PaymentTypeEnum.JKOPay,Name=ShopPaymentTypeEnum.PaymentTypeEnum.JKOPay.GetDescription()},
                    new() { Id=ShopPaymentTypeEnum.PaymentTypeEnum.CreditCard,Name=ShopPaymentTypeEnum.PaymentTypeEnum.CreditCard.GetDescription()},
                },
            };
        }
        public ShopInfoDto GetShopInfo(int id)
        {
            if (id == 0)
            {
                return new ShopInfoDto { PaymentTypes = new List<ShopPaymentTypeEnum.PaymentTypeEnum>() };
            }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                var paymentTypes = _repository.GetAll<ShopPaymentType>().Where(x => x.ShopId == id);
                var history = _repository.GetAll<ShopHistory>().Where(x => x.ShopId == id && x.UpdateTitle.Contains("ShopInfo"));
                return new ShopInfoDto
                {
                    UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                    Name = shop.Name,
                    Phone = shop.Phone,
                    City = shop.City,
                    District = shop.District,
                    Address = shop.Address,
                    HasSticker = shop.HasSticker,
                    PriceLimit = shop.PriceLimit,
                    PaymentTypes = paymentTypes.Select(x => (ShopPaymentTypeEnum.PaymentTypeEnum)x.PaymentType)
                };
            }
            catch (Exception ex)
            {
                return new ShopInfoDto { PaymentTypes = new List<ShopPaymentTypeEnum.PaymentTypeEnum>() };
            }
        }
        public ShopBusinessDto GetShopBusiness(int id)
        {
            if (id == 0)
            {
                return new ShopBusinessDto { OpenDayList = new List<string>() };
            }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                var history = _repository.GetAll<ShopHistory>().Where(x => x.ShopId == id && x.UpdateTitle.Contains("ShopBusiness"));
                return new ShopBusinessDto
                {
                    UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                    State = (ShopEnum.StateEnum)shop.State,
                    OpenDayList = shop.OpenDayList.Split(','),
                    OpenTime = shop.OpenTime != DateTime.MinValue ? shop.OpenTime.TransferToTaipeiTime().ToString("HH:mm:ss") : shop.OpenTime.ToString(),
                    CloseTime = shop.CloseTime != DateTime.MinValue ? shop.CloseTime.TransferToTaipeiTime().ToString("HH:mm:ss") : shop.CloseTime.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new ShopBusinessDto { OpenDayList = new List<string>() };
            }

        }
        public ShopTakeMethodDto GetShopTakeMethod(int id)
        {
            if (id == 0)
            {
                return new ShopTakeMethodDto { DeliveryConditions=new List<ShopTakeMethodDto.DeliveryCondition>() };
            }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                var takeMethods = _repository.GetAll<ShopMethod>().Where(x => x.ShopId == id);
                var history = _repository.GetAll<ShopHistory>().Where(x => x.ShopId == id && x.UpdateTitle.Contains("ShopTakeMethod"));
                return new ShopTakeMethodDto
                {
                    UpdateTime = history.Count() > 0 ? history.OrderBy(x => x.UpdateDate).Last().UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") : "",
                    PreOrder = shop.PreOrder,
                    IsTakeOut = takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut),
                    TakeOutTime = takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut) ? _repository.GetAll<Takeout>().First(x => x.ShopMethodId == takeMethods.First(y => y.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut).ShopMethodId).WaitingTime.TotalMinutes : 0,
                    IsDelivery = takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery),
                    DeliveryConditions = _repository.GetAll<Delivery>().Where(x => x.ShopMethodId == takeMethods.First(y => y.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery).ShopMethodId).Select(x => new ShopTakeMethodDto.DeliveryCondition
                    {
                        Id = x.DeliveryId,
                        PriceThreshold = x.PriceThreshold ?? 0,
                        LowerDistance = x.LowerDistance,
                        HigherDistance = x.HigherDistance,
                        DeliveryTime = x.WaitingTime == null ? 0 : x.WaitingTime.Value.TotalMinutes,
                        DeliveryFee = x.DeliveryFee ?? 0
                    })
                };
            }
            catch (Exception ex)
            {
                return new ShopTakeMethodDto { DeliveryConditions = new List<ShopTakeMethodDto.DeliveryCondition>() };
            }

        }
        public SaveDto CreateShop(int id, ShopInfoDto shopInfo)
        {
            if (shopInfo == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            try
            {
                if (id == 0)
                {
                    var shopPosition = GetShopPosition(shopInfo.City + shopInfo.District + shopInfo.Address).Result;
                    var shop = new Shop
                    {
                        BrandId = _userId,
                        Name = shopInfo.Name,
                        Phone = shopInfo.Phone,
                        City = shopInfo.City,
                        District = shopInfo.District,
                        Address = shopInfo.Address,
                        HasSticker = shopInfo.HasSticker,
                        PriceLimit = shopInfo.PriceLimit,
                        Lat = shopPosition.Lat,
                        Lng = shopPosition.Lng,
                        PreOrder = false,
                        State = (int)ShopEnum.StateEnum.Close,
                        OpenDayList = "",
                        VerificationId = _repository.GetAll<Brand>().First(x => x.BrandId == _userId).VerificationId,
                        OpenTime = DateTime.MinValue,
                        CloseTime = DateTime.MinValue,
                        Verified = true,
                        Star = 0,
                        StarAmount = 0
                    };
                    _repository.Create(shop);
                    _repository.Save();
                    var shopId = shop.ShopId;
                    foreach (var type in shopInfo.PaymentTypes)
                    {
                        _repository.Create(new ShopPaymentType
                        {
                            ShopId = shopId,
                            PaymentType = (int)type
                        });
                    }
                    var createShop = new CreateShopDto
                    {
                        ShopId = shopId,
                        BrandId = shop.BrandId,
                        Name = shop.Name,
                        Phone = shop.Phone,
                        City = shop.City,
                        District = shop.District,
                        Address = shop.Address,
                        Lat = shop.Lat,
                        Lng = shop.Lng,
                        OpenDayList = shop.OpenDayList,
                        OpenTime = shop.OpenTime,
                        CloseTime = shop.CloseTime,
                        HasSticker = shop.HasSticker,
                        PreOrder = shop.PreOrder,
                        PriceLimit = shop.PriceLimit,
                        State = shop.State,
                        StarAmount = shop.StarAmount,
                        Star = shop.Star,
                        VerificationId = shop.VerificationId,
                        Verified = shop.Verified,
                        PaymentTypes = shopInfo.PaymentTypes
                    };
                    var shopHistory = new ShopHistory { ShopId = shopId, UpdateDate = DateTime.UtcNow, UpdateTitle = "CreateShop", UpdateContent = createShop.JsonSerialize() };
                    _repository.Create(shopHistory);
                    _repository.Create(new ShopHistory { ShopId = shopId, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopInfo", UpdateContent = shopInfo.JsonSerialize() });
                    _repository.Create(new ShopHistory { ShopId = shopId, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopBusiness", UpdateContent = "" });
                    _repository.Create(new ShopHistory { ShopId = shopId, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopTakeMethod", UpdateContent = "" });
                    _repository.Save();
                    return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = shopHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss"), NewShop = new SaveDto.Shop { Id = shopId, Name = shopInfo.Name } };
                }
                else { return new SaveDto { IsSuccess = false, Message = "此商店已存在 保存失敗" }; }
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public SaveDto RemoveShop(int id)
        {
            if (id == 0)
            {
                return new SaveDto { IsSuccess = false, Message = "此商店不存在 刪除失敗" };
            }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                shop.State = (int)ShopEnum.StateEnum.Remove;
                _repository.Update(shop);
                var shopHistory = new ShopHistory { ShopId = id, UpdateDate = DateTime.UtcNow, UpdateTitle = "RemoveShop", UpdateContent = shop.JsonSerialize() };
                _repository.Create(shopHistory);
                _repository.Create(new ShopHistory { ShopId = id, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopInfo", UpdateContent = "" });
                _repository.Create(new ShopHistory { ShopId = id, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopBusiness", UpdateContent = "" });
                _repository.Create(new ShopHistory { ShopId = id, UpdateDate = shopHistory.UpdateDate, UpdateTitle = "ShopTakeMethod", UpdateContent = "" });
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "刪除成功" };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 刪除失敗" };
            }
        }
        public SaveDto SaveShopInfo(int id, ShopInfoDto shopInfo)
        {
            if (shopInfo == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            if (id == 0) { return new SaveDto { IsSuccess = false, Message = "此商店不存在 保存失敗" }; }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                if (shopInfo.City + shopInfo.District + shopInfo.Address != shop.City + shop.District + shop.Address)
                {
                    var shopPosition = GetShopPosition(shopInfo.City + shopInfo.District + shopInfo.Address).Result;
                    shop.Lat = shopPosition.Lat;
                    shop.Lng = shopPosition.Lng;
                }
                shop.Name = shopInfo.Name;
                shop.Phone = shopInfo.Phone;
                shop.City = shopInfo.City;
                shop.District = shopInfo.District;
                shop.Address = shopInfo.Address;
                shop.HasSticker = shopInfo.HasSticker;
                shop.PriceLimit = shopInfo.PriceLimit;
                _repository.Update(shop);
                var paymentTypes = _repository.GetAll<ShopPaymentType>().Where(x => x.ShopId == id).ToList();
                var removePaymentTypes = paymentTypes.Select(x => (ShopPaymentTypeEnum.PaymentTypeEnum)x.PaymentType).Except(shopInfo.PaymentTypes);
                if (removePaymentTypes.Count() > 0)
                {
                    foreach (var p in paymentTypes.Where(x => removePaymentTypes.Contains((ShopPaymentTypeEnum.PaymentTypeEnum)x.PaymentType)))
                    {
                        _repository.Delete(p);
                    }
                }
                var addPaymentTypes = shopInfo.PaymentTypes.Except(paymentTypes.Select(x => (ShopPaymentTypeEnum.PaymentTypeEnum)x.PaymentType));
                if (addPaymentTypes.Count() > 0)
                {
                    foreach (var p in addPaymentTypes)
                    {
                        _repository.Create(new ShopPaymentType { ShopId = id, PaymentType = (int)p });
                    }
                }

                var shopHistory = new ShopHistory { ShopId = id, UpdateDate = DateTime.UtcNow, UpdateTitle = "ShopInfo", UpdateContent = shopInfo.JsonSerialize() };
                _repository.Create(shopHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = shopHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public SaveDto SaveShopBusiness(int id, ShopBusinessDto shopBusiness)
        {
            if (shopBusiness == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            if (id == 0) { return new SaveDto { IsSuccess = false, Message = "此商店不存在 保存失敗" }; }
            try
            {
                var taipeiZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                shop.State = (int)shopBusiness.State;
                shop.OpenDayList = String.Join(",", shopBusiness.OpenDayList);
                shop.OpenTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse("2000-01-01 " + shopBusiness.OpenTime), taipeiZone);
                shop.CloseTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse("2000-01-01 " + shopBusiness.CloseTime), taipeiZone);
                _repository.Update(shop);
                var shopHistory = new ShopHistory { ShopId = id, UpdateDate = DateTime.UtcNow, UpdateTitle = "ShopBusiness", UpdateContent = shopBusiness.JsonSerialize() };
                _repository.Create(shopHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = shopHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        public SaveDto SaveShopTakeMethod(int id, ShopTakeMethodDto shopTakeMethod)
        {
            if (shopTakeMethod == null)
            {
                return new SaveDto { IsSuccess = false, Message = "未取得資料 保存失敗" };
            }
            if (id == 0) { return new SaveDto { IsSuccess = false, Message = "此商店不存在 保存失敗" }; }
            try
            {
                var shop = _repository.GetAll<Shop>().First(x => x.BrandId == _userId && x.ShopId == id);
                shop.PreOrder = shopTakeMethod.PreOrder;
                _repository.Update(shop);
                var takeMethods = _repository.GetAll<ShopMethod>().Where(x => x.ShopId == id);
                if (shopTakeMethod.IsTakeOut)
                {
                    if (takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut))
                    {
                        var takeout = _repository.GetAll<Takeout>().First(x => x.ShopMethodId == takeMethods.First(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut).ShopMethodId);
                        takeout.WaitingTime = TimeSpan.FromMinutes(shopTakeMethod.TakeOutTime);
                        _repository.Update(takeout);
                    }
                    else
                    {
                        _repository.Create(new ShopMethod
                        {
                            ShopId = id,
                            TakeMethod = (int)ShopMethodEnum.TakeMethodEnum.TakeOut
                        });
                        _repository.Save();
                        var takeout = _repository.GetAll<ShopMethod>().First(x => x.ShopId == id && x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut);
                        _repository.Create(new Takeout
                        {
                            ShopMethodId = takeout.ShopMethodId,
                            WaitingTime = TimeSpan.FromMinutes(shopTakeMethod.TakeOutTime)
                        });
                    }
                }
                else
                {
                    if (takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut))
                    {
                        var takeMethod = takeMethods.First(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.TakeOut);
                        var takeout = _repository.GetAll<Takeout>().First(x => x.ShopMethodId == takeMethod.ShopMethodId);
                        _repository.Delete(takeout);
                        _repository.Delete(takeMethod);
                    }
                }
                if (shopTakeMethod.IsDelivery)
                {
                    if (takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery))
                    {
                        var shopMethodId = takeMethods.First(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery).ShopMethodId;
                        var deliverys = _repository.GetAll<Delivery>().Where(x => x.ShopMethodId == shopMethodId).ToList();
                        var removeDeliverys = deliverys.Select(x => x.DeliveryId).Except(shopTakeMethod.DeliveryConditions.Select(x => x.Id));
                        if (removeDeliverys.Count() > 0)
                        {
                            foreach (var d in deliverys.Where(x => removeDeliverys.Contains(x.DeliveryId)))
                            {
                                _repository.Delete(d);
                            }
                        }
                        var addDeliverys = shopTakeMethod.DeliveryConditions.Where(x => x.Id == 0);
                        if (addDeliverys.Count() > 0)
                        {
                            foreach (var d in addDeliverys)
                            {
                                var low = d.LowerDistance;
                                var high = d.HigherDistance;
                                if (d.HigherDistance != null)
                                {
                                    if (d.LowerDistance > d.HigherDistance)
                                    {
                                        low = d.HigherDistance??0;
                                        high = d.LowerDistance;
                                    }
                                }
                                _repository.Create(new Delivery
                                {
                                    ShopMethodId = shopMethodId,
                                    PriceThreshold = d.PriceThreshold,
                                    LowerDistance = low,
                                    HigherDistance = high,
                                    WaitingTime = TimeSpan.FromMinutes(d.DeliveryTime),
                                    DeliveryFee = d.DeliveryFee
                                });
                            }
                        }
                        var updateDeliverys = deliverys.Select(x => x.DeliveryId).Intersect(shopTakeMethod.DeliveryConditions.Select(x => x.Id));
                        if (updateDeliverys.Count() > 0)
                        {
                            foreach (var d in deliverys.Where(x => updateDeliverys.Contains(x.DeliveryId)))
                            {
                                var delivery = shopTakeMethod.DeliveryConditions.First(x => x.Id == d.DeliveryId);
                                var low = delivery.LowerDistance;
                                var high = delivery.HigherDistance;
                                if (delivery.HigherDistance != null)
                                {
                                    if (delivery.LowerDistance > delivery.HigherDistance)
                                    {
                                        low = delivery.HigherDistance ?? 0;
                                        high = delivery.LowerDistance;
                                    }
                                }
                                d.PriceThreshold = delivery.PriceThreshold;
                                d.LowerDistance = low;
                                d.HigherDistance = high;
                                d.WaitingTime = TimeSpan.FromMinutes(delivery.DeliveryTime);
                                d.DeliveryFee = delivery.DeliveryFee;
                                _repository.Update(d);
                            }
                        }
                    }
                    else
                    {
                        _repository.Create(new ShopMethod
                        {
                            ShopId = id,
                            TakeMethod = (int)ShopMethodEnum.TakeMethodEnum.Delivery
                        });
                        _repository.Save();
                        var delivery = _repository.GetAll<ShopMethod>().First(x => x.ShopId == id && x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery);
                        foreach (var d in shopTakeMethod.DeliveryConditions)
                        {
                            var low = d.LowerDistance;
                            var high = d.HigherDistance;
                            if (d.HigherDistance != null)
                            {
                                if (d.LowerDistance > d.HigherDistance)
                                {
                                    low = d.HigherDistance ?? 0;
                                    high = d.LowerDistance;
                                }
                            }
                            _repository.Create(new Delivery
                            {
                                ShopMethodId = delivery.ShopMethodId,
                                PriceThreshold = d.PriceThreshold,
                                LowerDistance = low,
                                HigherDistance = high,
                                WaitingTime = TimeSpan.FromMinutes(d.DeliveryTime),
                                DeliveryFee = d.DeliveryFee,
                            });
                        }

                    }
                }
                else
                {
                    if (takeMethods.Any(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery))
                    {
                        var takeMethod = takeMethods.First(x => x.TakeMethod == (int)ShopMethodEnum.TakeMethodEnum.Delivery);
                        var deliverys = _repository.GetAll<Delivery>().Where(x => x.ShopMethodId == takeMethod.ShopMethodId);
                        foreach (var delivery in deliverys)
                        {
                            _repository.Delete(delivery);
                        }
                        _repository.Delete(takeMethod);
                    }
                }
                var shopHistory = new ShopHistory { ShopId = id, UpdateDate = DateTime.UtcNow, UpdateTitle = "ShopTakeMethod", UpdateContent = shopTakeMethod.JsonSerialize() };
                _repository.Create(shopHistory);
                _repository.Save();
                return new SaveDto { IsSuccess = true, Message = "保存成功", UpdateTime = shopHistory.UpdateDate.TransferToTaipeiTime().ToString(@"yyyy/MM/dd HH:mm:ss") };
            }
            catch (Exception ex)
            {
                return new SaveDto { IsSuccess = false, Message = $"{ex.Message} 保存失敗" };
            }
        }
        private async Task<ShopPositionDto> GetShopPosition(string address)
        {
            var api = _config["GoogleMap:Api"];
            var key = _config["GoogleMap:Key"];
            var requestUri = @$"{api}address={address}&key={key}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Accept", "application/json");
            var client = _httpClient.CreateClient();
            var response = client.Send(request);
            var position = JsonSerializer.DeserializeAsync<PositionDto>(await response.Content.ReadAsStreamAsync()).Result;
            var shopPosition = new ShopPositionDto { Lat = (double)position.results[0].geometry.location.lat, Lng = (double)position.results[0].geometry.location.lng };
            return shopPosition;
        }

    }
}
