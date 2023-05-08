using AutoMapper;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Member;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Services
{
    public class MemberService : IMemberService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClient;
        public MemberService(IRepository repository, IMapper mapper, IConfiguration config, IHttpClientFactory httpClient)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
            _httpClient = httpClient;
        }
        //public APIResult GetCardGradingData(int brandId)
        public IEnumerable<GetCardGradingDto> GetCardGradingData(int brandId)
        {
            
            var cardGradingData = _repository.GetAll<CardType>().Where(x => x.BrandId == brandId).ToList();
            var resultList = cardGradingData.Select(x => _mapper.Map<GetCardGradingDto>(x));
            return resultList.OrderBy(x => x.Range);

            #region try catch
            //try
            //{
            //    var cardGradingData = _repository.GetAll<CardType>().Where(x => x.BrandId == brandId).ToList();
            //    var data = cardGradingData.Select(x => _mapper.Map<GetCardGradingDto>(x));
            //    return new APIResult(APIStatus.Success, "卡片資料讀取成功", data);
            //}
            //catch (Exception ex)
            //{
            //    //_logger.LogError(ex, "卡片資料讀取失敗");
            //    return new APIResult(APIStatus.Fail, "卡片資料讀取失敗", null);
            //}
            #endregion
        }
        public APIResult CreateCardGrading(CreateCardCradingDto input, int brandId)
        {
            //檢核
            //if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.Name))

            if (_repository.GetAll<CardType>().Any(x => x.Name == input.Name && x.BrandId == brandId))
            {
                return new APIResult(APIStatus.Fail, "名稱不可重複", null);
            }

            //if (_repository.GetAll<CardType>().Any(x => x.CardImgUrl == input.CardImgUrl && x.BrandId == brandId))
            //{
            //    return new APIResult(APIStatus.Fail, "卡片圖片不可重複", null);
            //}

            if (_repository.GetAll<CardType>().Any(x => x.Range == input.Range && x.BrandId == brandId))
            {
                return new APIResult(APIStatus.Fail, "消費次數不可重複", null);
            }

            var entity = _mapper.Map<CardType>(input);
            entity.CardLevel = 1;

            try
            {
                _repository.Create(entity);
                _repository.Save();
                return new APIResult(APIStatus.Success, string.Empty, null);
            }
            catch (Exception ex)
            {
                return new APIResult(APIStatus.Fail, ex.ToString(), null);
            }
        }
        public APIResult DeleteCard(DeleteCardDto input)
        {
            var entity = _repository.GetAll<CardType>().First(x => x.CardTypeId == input.CardTypeId);
            try
            {
                _repository.Delete(entity);
                _repository.Save();
                return new APIResult(APIStatus.Success, string.Empty, null);
            }
            catch(Exception ex)
            {
                return new APIResult(APIStatus.Fail, ex.ToString(), null);
            }
        }
        public APIResult UpdateCard(UpdateCardDto input)
        {
            var entity = _repository.GetAll<CardType>().First(x => x.CardTypeId == input.CardTypeId);
            entity.Name = input.Name;
            entity.Range = input.Range;
            entity.CardImgUrl = input.CardImgUrl;

            try
            {
                _repository.Update(entity);
                _repository.Save();
                return new APIResult(APIStatus.Success, string.Empty, null);
            }
            catch (Exception ex)
            {
                return new APIResult(APIStatus.Fail, ex.ToString(), null);
            }
        }
        public async Task<APIResult> UploadImg(IFormFile file)
        {
            var fileType = new List<string> { "image/jpeg", "image/png", "image/jpg" };

            if (file == null || file.Length == 0){
                return new APIResult(APIStatus.Fail, "未選擇檔案", null);
            }
            if (!fileType.Contains(file.ContentType))
            {
                return new APIResult(APIStatus.Fail, "檔案類型不合規", null);
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
                return new APIResult(APIStatus.Success, "上傳成功", imgUpload.Link);
            }
            catch(Exception ex)
            {
                return new APIResult(APIStatus.Fail, ex.ToString(), null);
            }
        }
        public APIResult GetBrandMemberData(int brandId)
        {
            try
            {
                var userCardList = GetAllUserCardByBrandId(brandId);
                var userData = GetAllUserData(userCardList);
                var brandMemberData = GetCardType(brandId, userData);
                return new APIResult(APIStatus.Success, string.Empty, brandMemberData);
            }
            catch (Exception ex)
            {
                return new APIResult(APIStatus.Fail, ex.ToString(), null);
            }
        }
        
        private IEnumerable<GetAllUserCardByBrandIdDto> GetAllUserCardByBrandId(int brandId)
        {
            //找出該品牌所有分店Id
            var shopIdList = _repository.GetAll<Shop>().Where(x => x.BrandId == brandId).Select(y => new
            {
                ShopId = y.ShopId
            }).ToList();

            //找出所有訂單
            var orderList = _repository.GetAll<Order>().ToList();
            //找分店的訂單
            var shopOrderList = new List<Order>();
            foreach (var order in orderList)
            {
                if (shopIdList.Any(x => x.ShopId == order.ShopId))
                {
                    shopOrderList.Add(order);
                }
            }
            //groupby UserAccountId，並計算每位會員各有幾筆訂單
            var result = shopOrderList.GroupBy(x => x.UserAcountId).Select(x => new GetAllUserCardByBrandIdDto()
            {
                BrandId = brandId,
                UserAccountId = x.Key,
                Points = x.Count()
            });
            return result.OrderBy(x => x.UserAccountId);
        }
        private IEnumerable<GetAllUserDataDto> GetAllUserData(IEnumerable<GetAllUserCardByBrandIdDto> userCardList)
        {
            var userNickName = _repository.GetAll<UserAccount>().Select(x => new UserNickName()
            {
                UserAccountId = x.UserAccountId,
                NickName = x.NickName
            }).ToList();

            //將NickName和usercardlist依據useraccountId合併
            var result =
                from nickName in userNickName
                join card in userCardList
                on nickName.UserAccountId equals card.UserAccountId
                select new GetAllUserDataDto()
                {
                    UserAccountId = card.UserAccountId,
                    NickName = nickName.NickName,
                    BrandId = card.BrandId,
                    Points = card.Points
                };
            return result;
        }
        private IEnumerable<GetBrandMemberDataDto> GetCardType(int brandId, IEnumerable<GetAllUserDataDto> userData)
        {
            //用brandId抓該品牌的CardType、Range
            //取得該品牌卡片的卡片類型資料
            var cardtypeList = _repository.GetAll<CardType>().Where(x => x.BrandId == brandId).Select(x => new GetCardTypesDto()
            {
                BrandId = x.BrandId,
                CardType = x.Name,
                Range = x.Range == null ? 0 : x.Range.Value,
            }).ToList();

            List<GetBrandMemberDataDto> brandMemberData = new List<GetBrandMemberDataDto>();

            //取得門檻資料
            var rangeList = cardtypeList.Select(x => x.Range).ToList();
            if (rangeList != null)
            {
                //判斷他屬於哪個等級
                foreach (var card in userData)
                {
                    //卡片點數
                    var points = card.Points;
                    //該點數對應的range
                    var targetRange = rangeList.First(x => points - x <= 0);
                    //該點數對應等級的名稱
                    var targetCardType = cardtypeList.First(x => x.Range == targetRange).CardType;
                    //放入Dto
                    brandMemberData.Add(new GetBrandMemberDataDto() { 
                        UserAccountId = card.UserAccountId, 
                        BrandId = card.BrandId, 
                        NickName = card.NickName,
                        Point = points,
                        GradeName = targetCardType
                    });
                }
            }
            return brandMemberData;

        }

        
    }
}
