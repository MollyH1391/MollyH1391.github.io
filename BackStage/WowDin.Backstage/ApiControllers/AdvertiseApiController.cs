using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Advertise;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.ApiControllers
{
    [ApiController]
    public class AdvertiseApiController : ControllerBase
    {
        private readonly IAdvertiseService _advertiseService;
        private readonly IMapper _mapper;

        public AdvertiseApiController(IAdvertiseService advertiseService, IMapper mapper)
        {
            _advertiseService = advertiseService;
            _mapper = mapper;
        }

        [Authorize]
        [Route("api/Advertise/{brandId}")]
        [HttpGet]

        public IActionResult GetAllAdvertise(int brandId)
        {
            var adsDto = _advertiseService.GetAllAdvertise(brandId);

            return Ok(new APIResult(APIStatus.Success, "讀取成功", _mapper.Map<AllAdvertiseVM>(adsDto)));
        }

        [Authorize]
        [Route("api/Advertise")]
        [HttpPost]
        public IActionResult SubmitApplication(AdvertiseRequestVM request)
        {
            var requestDto = _mapper.Map<AdvertiseRequestDto>(request);

            var result = _advertiseService.SubmitApplication(requestDto);

            return Ok(result);
        }

        [Authorize]
        [Route("api/ReSubmit/{adId}")]
        [HttpGet]
        public IActionResult ReSubmit(int adId)
        {
            var result = _advertiseService.ReSubmit(adId);

            return Ok(result);
        }
    }
}
