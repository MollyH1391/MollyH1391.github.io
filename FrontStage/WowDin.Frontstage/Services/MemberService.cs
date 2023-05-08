using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models.Dto;
using WowDin.Frontstage.Models.Dto.Home;
using WowDin.Frontstage.Models.Dto.Member;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Repositories.Interface;
using WowDin.Frontstage.Services.Member.Interface;

namespace WowDin.Frontstage.Services.Member
{
    public class MemberService : IMemberService
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClient;
        public MemberService(IRepository repository, IConfiguration config, IHttpClientFactory httpClient)
        {
            _repository = repository;
            _config = config;
            _httpClient = httpClient;
        }

        public ResponseDto InitialResponse(int userId)
        {
            var person = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == userId);
            var brandlist = _repository.GetAll<Brand>();
            var shoplist = _repository.GetAll<Shop>();
            var result = new ResponseDto()
            {
                
                Brand = brandlist.Select(x => new Brands()
                {
                    BrandId = x.BrandId,
                    BrandName = x.Name,
                    Shop = shoplist.Where(y => y.BrandId == x.BrandId).Select(z=>new Shops() 
                    {
                        ShopId = z.ShopId,
                        ShopName = z.Name
                    }).ToList()
                    //shoplist.Where(y => y.BrandId == x.BrandId).Select(z=> z.Name).ToList()
                }).ToList(),
                Phone = person.Phone,
                Name = person.RealName
                
            };
            return result;
        }

        public void AddReponse(AddResponseDto addResponseData)
        {
            var response = new Response()
            {
                UserAccountId = addResponseData.UserAccountId,
                BrandId = addResponseData.BrandId,
                ShopId = addResponseData.ShopId,
                ResponseContent = addResponseData.ResponseContent,
                Date = DateTime.UtcNow.TransferToTaipeiTime(),
            };
            _repository.Create(response);
            _repository.Save();
        }

        public GetUserDataByIdDto GetUserDataById(int id)
        {
            var user = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == id);
            var points = _repository.GetAll<Models.Entity.Order>().Where(x => x.UserAcountId == id).Count();
            //var points = _repository.GetAll<UserCard>().Where(x => x.UserAccountId == id).Sum(x => x.Points);
            var couponAmount = _repository.GetAll<CouponContainer>().Where(x => x.UserAccountId == id).Count();
            

            return new GetUserDataByIdDto()
            {
                UserAccountId = user.UserAccountId,
                LoginType = user.LoginType,
                RealName = user.RealName,
                NickName = user.NickName,
                Photo = user.Photo,
                //Sex = ((UserAccountEnum.SexEnum)user.Sex).GetDescription(),
                Sex = (int)user.Sex,
                Birthday = user.Birthday.HasValue ? user.Birthday.Value : DateTime.Now,
                Phone = user.Phone,
                City = user.City,
                Distrinct = user.Distrinct,
                Point = points,
                CouponAmount = couponAmount
            };
        }

        public IEnumerable<GetCardListByIdDto> GetCardListById(int id)
        {
            //找到該會員所有訂單
            var orderList = _repository.GetAll<Models.Entity.Order>().Where(x => x.UserAcountId == id).ToList();
            //統計每個shopId分別有幾筆訂單
            var orderListGroupByShop = orderList.GroupBy(x => x.ShopId).Select(x => new
            {
                ShopId = x.Key,
                Frequency = x.Count()
            });
            //找到BrandId
            var orderListToBrandId = orderListGroupByShop.Select(x => new
            {
                BrandId = _repository.GetAll<Shop>().First(y => y.ShopId == x.ShopId).BrandId,
                x.Frequency
            });

            //再次以BrandId groupby
            var orderListGroupByBrandId = orderListToBrandId.GroupBy(x => x.BrandId)
                                                            .Select(y => new
                                                            {
                                                                BrandId = y.Key,
                                                                Frequency = y.Sum(z => z.Frequency)
                                                            });

            //取得該會員所有會員卡
            //var userCards = _repository.GetAll<UserCard>().Where(x => x.UserAccountId == id).ToList();

            var cardList = new List<GetCardListByIdDto>();
            foreach (var card in orderListGroupByBrandId)
            {
                //取得每張會員卡的品牌ID
                var brandId = card.BrandId;

                //取得該品牌卡片的卡片類型資料
                var brandCardtypeList = _repository.GetAll<CardType>().Where(x => x.BrandId == brandId);
                if (brandCardtypeList != null)
                {
                    //取得該品牌的卡片等級的rangeList
                    var rangeList = brandCardtypeList.Select(x => x.Range).ToList();
                    if (rangeList.Count != 0)
                    {
                        //該卡片的點數
                        //取得該會員在該brandId的消費次數
                        var point = card.Frequency;
                        //該點數對應的等級
                        var targetRange = rangeList.First(x => point - x <= 0);


                        string nextCardTypeName;
                        //該點數對應的等級的index
                        var nextRangeIndex = rangeList.IndexOf(rangeList.First(x => point - x <= 0)) + 1;
                        //下個等級的index
                        var nextTargetRange = rangeList.ElementAtOrDefault(nextRangeIndex);
                        if (nextTargetRange != null)
                        {
                            nextCardTypeName = brandCardtypeList.First(x => x.Range == nextTargetRange).Name;

                        }
                        else
                        {
                            nextCardTypeName = "無下一等級";

                        }


                        //找到該等級的卡片圖片
                        var cardImgUrl = brandCardtypeList.First(x => x.Range == targetRange).CardImgUrl;
                        //找到該等級的名稱
                        var cardTypeName = brandCardtypeList.First(x => x.Range == targetRange).Name;


                        cardList.Add(new GetCardListByIdDto()
                        {
                            UserAccountId = id,
                            BrandId = brandId,
                            CardImgUrl = cardImgUrl,
                            CardTypeName = cardTypeName,
                            NextCardTypeName = nextCardTypeName,
                            Range = (int)targetRange,
                            Point = point
                        });
                    }
                }
                else
                {
                    break;
                }
            }
            return cardList;
        }

        public bool UpdateUserData(UpdateUserDataDto userData)
        {
            var isExist = GetUserDataById(userData.UserAccountId);
            if (isExist != null)
            {
                //var editUser = new UserAccount()
                //{
                //    UserAccountId = userData.UserAccountId,
                //    RealName = userData.RealName,
                //    NickName = userData.NickName,
                //    Photo = userData.Photo,
                //    Sex = userData.Sex,
                //    Birthday = userData.Birthday,
                //    Phone = userData.Phone,
                //    City = userData.City,
                //    Distrinct = userData.Distrinct

                //};
                var editUser = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == userData.UserAccountId);
                editUser.RealName = userData.RealName;
                editUser.NickName = userData.NickName;
                editUser.Photo = userData.Photo;
                editUser.Sex = userData.Sex;
                editUser.Birthday = userData.Birthday;
                editUser.Phone = userData.Phone;
                editUser.City = userData.City;
                editUser.Distrinct = userData.Distrinct;

                _repository.Update(editUser);
                _repository.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UploadImgOutputDto> UploadImg(IFormFile file)
        {
            var fileType = new List<string> { "image/jpeg", "image/png", "image/jpg" };

            if (file == null || file.Length == 0)
            {
                return new UploadImgOutputDto(false, "未選擇檔案", null);
            }
            if (!fileType.Contains(file.ContentType))
            {
                return new UploadImgOutputDto(false, "檔案類型不合規", null);
            }

            try
            {
                var img = file.OpenReadStream();
                var clientId = _config["Imgur_member:ClientId"];
                var clientSecret = _config["Imgur_member:ClientSecret"];
                var apiClient = new ApiClient(clientId, clientSecret);
                var httpClient = _httpClient.CreateClient();
                var endpoint = new ImageEndpoint(apiClient, httpClient);
                var imgUpload = await endpoint.UploadImageAsync(img);
                return new UploadImgOutputDto(true, "上傳成功", imgUpload.Link);
            }
            catch (Exception ex)
            {
                return new UploadImgOutputDto(true, ex.ToString(), null);
            }
        }
    }
}
