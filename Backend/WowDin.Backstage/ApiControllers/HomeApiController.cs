using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WowDin.Backstage.Models;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.ApiControllers
{
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeApiController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [Route("api/GetChartDataById")]
        [HttpGet]
        public IActionResult GetChartDataById(int brandId)
        {
            try
            {
                brandId = int.Parse(User.Identity.Name);
                var result = _homeService.GetChartDataById(brandId);
                return Ok(new APIResult(APIStatus.Success, string.Empty, result));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, null));
            }
        }

    }
}
