using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Backstage.Common.ModelEnum;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Advertise;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Services
{
    public class AdvertiseService : IAdvertiseService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponService> _logger;

        public AdvertiseService(IRepository repository, IMapper mapper, ILogger<CouponService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper; 
        }

        public AllAdvertiseDto GetAllAdvertise(int brandId)
        {
            var ads = _repository.GetAll<Advertise>().Where(x => x.BrandId == brandId).ToList();
            var coupons = _repository.GetAll<Coupon>().Where(x => ads.Select(a => a.CouponId).Contains(x.CouponId)).Distinct().ToList();

            var result = new AllAdvertiseDto()
            {
                AllAdvertise = new List<AdvertiseDto>()
            };

            foreach (var ad in ads)
            {
                var dto = _mapper.Map<AdvertiseDto>(ad);

                if(ad.CouponId != null)
                {
                    dto.CouponCode = coupons.First(x => x.CouponId == ad.CouponId).Code;
                }

                result.AllAdvertise.Add(dto);
            }

            return result;
        }

        public APIResult SubmitApplication(AdvertiseRequestDto request)
        {
            try
            {
                var entity = _mapper.Map<Advertise>(request);
                if(request.CouponId == 0)
                {
                    entity.CouponId = null;
                }
                entity.Status = (int)AdvertiseEnum.StatusEnum.Pending;

                _repository.Create(entity);
                _repository.Save();
                
                return new APIResult(APIStatus.Success, "提交成功", null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "廣告提交失敗");
                return new APIResult(APIStatus.Fail, "提交失敗", null);
            }
        }

        public APIResult ReSubmit(int adId)
        {
            try
            {
                var entity = _repository.GetAll<Advertise>().First(x => x.AdvertiseId == adId);
                entity.Status = (int)AdvertiseEnum.StatusEnum.Pending;

                _repository.Update(entity);
                _repository.Save();

                return new APIResult(APIStatus.Success, "提交成功", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "廣告提交失敗");
                return new APIResult(APIStatus.Fail, "提交失敗", null);
            }
        }

    }
}
