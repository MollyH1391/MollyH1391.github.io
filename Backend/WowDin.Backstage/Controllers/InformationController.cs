
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Backstage.Models.Dto.Information;
using WowDin.Backstage.Models.ViewModel.Information;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using WowDin.Backstage.Common.ModelEnum;
using AutoMapper;

namespace WowDin.Backstage.Controllers
{
    public class InformationController : Controller
    {
        private readonly IInformationService _informationService;
        private readonly IMapper _mapper;
        public InformationController(IInformationService informationService,IMapper mapper)
        {
            _informationService = informationService;
            _mapper=mapper;
        }

        [Authorize]
        public IActionResult BrandManagement()
        {
            var brandManagementDto = _informationService.BrandManagementInit();
            var brandManagementVM = new BrandManagementViewModel
            {
                BrandFacade = _mapper.Map<BrandFacadeViewModel>(_informationService.GetBrandFacade()),
                BrandIntroduce = _mapper.Map<BrandIntroduceViewModel>(_informationService.GetBrandIntroduce()),
                BrandWeb = _mapper.Map<BrandWebViewModel>(_informationService.GetBrandWeb()),
                Star = brandManagementDto.Star,
                Categories = brandManagementDto.Categories.Select(x => new BrandManagementViewModel.CategoryOp
                {
                    text = x.Name,
                    value = new BrandManagementViewModel.Category
                    {
                        Id = x.Id,
                        Image = x.Image
                    },
                    disabled = false
                }).ToList(),
                BrandImages = _mapper.Map<BrandImagesViewModel>(_informationService.GetBrandImages()),
                ShopImages=_mapper.Map<ShopImagesViewModel>(_informationService.GetShopImages())
            }.JsonSerialize();
            return View("BrandManagement", brandManagementVM);
        }

        [Authorize]
        public IActionResult ShopManagement()
        {
            var shopManagementVM = _mapper.Map<ShopManagementViewModel>(_informationService.ShopManagementInit()).JsonSerialize();
            return View("ShopManagement",shopManagementVM);
        }
        [Authorize]
        [HttpGet]
        [Route("/Information/CreateShop/{id}")]
        public IActionResult CreateShop(int id)
        {
            var shopVM = new ShopViewModel
            {
                ShopInfo = new ShopInfoViewModel() { UpdateTime = DateTime.Now.ToString(@"yyyy/MM/dd HH:mm:ss"), PaymentTypes = new List<ShopPaymentTypeEnum.PaymentTypeEnum>() },
                ShopBusiness = new ShopBusinessViewModel() { State=ShopEnum.StateEnum.Close,OpenDayList = new List<string>() },
                ShopTakeMethod = new ShopTakeMethodViewModel() { PreOrder=false,IsTakeOut=false,IsDelivery=false,DeliveryConditions = new List<ShopTakeMethodViewModel.DeliveryCondition>() }
            }.JsonSerialize();
            return new JsonResult(shopVM);
        }
        [Authorize]
        [HttpPost]
        [Route("/Information/CreateShop/{id}")]
        public IActionResult CreateShop(int id,[FromBody]ShopInfoViewModel shopInfo)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.CreateShop(id, _mapper.Map<ShopInfoDto>(shopInfo))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [Route("/Information/RemoveShop/{id}")]
        public IActionResult RemoveShop(int id)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.RemoveShop(id)).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [Route("/Information/ShowShop/{id}")]
        public IActionResult ShowShop(int id)
        {
            var shopVM = new ShopViewModel
            {
                ShopInfo = _mapper.Map<ShopInfoViewModel>(_informationService.GetShopInfo(id)),
                ShopBusiness = _mapper.Map<ShopBusinessViewModel>(_informationService.GetShopBusiness(id)),
                ShopTakeMethod = _mapper.Map<ShopTakeMethodViewModel>(_informationService.GetShopTakeMethod(id))
            }.JsonSerialize();
            return new JsonResult(shopVM);
        }
        [Authorize]
        [HttpGet]
        [Route("/information/ShopInfo/{id}")]
        public IActionResult ShopInfo(int id)
        {
            var shopInfoVM = _mapper.Map<ShopInfoViewModel>(_informationService.GetShopInfo(id)).JsonSerialize();
            return new JsonResult(shopInfoVM);
        }
        [Authorize]
        [HttpPost]
        [Route("/information/ShopInfo/{id}")]
        public IActionResult ShopInfo(int id,[FromBody]ShopInfoViewModel shopInfo)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.SaveShopInfo(id, _mapper.Map<ShopInfoDto>(shopInfo))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        [Route("/information/ShopBusiness/{id}")]
        public IActionResult ShopBusiness(int id)
        {
            var shopBusinessVM = _mapper.Map<ShopBusinessViewModel>(_informationService.GetShopBusiness(id)).JsonSerialize();
            return new JsonResult(shopBusinessVM);
        }
        [Authorize]
        [HttpPost]
        [Route("/information/ShopBusiness/{id}")]
        public IActionResult ShopBusiness(int id,[FromBody] ShopBusinessViewModel shopBusiness)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.SaveShopBusiness(id, _mapper.Map<ShopBusinessDto>(shopBusiness))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        [Route("/information/ShopTakeMethod/{id}")]
        public IActionResult ShopTakeMethod(int id)
        {
            var shopTakeMethodVM = _mapper.Map<ShopTakeMethodViewModel>(_informationService.GetShopTakeMethod(id)).JsonSerialize();
            return new JsonResult(shopTakeMethodVM);
        }
        [Authorize]
        [HttpPost]
        [Route("/information/ShopTakeMethod/{id}")]
        public IActionResult ShopTakeMethod(int id,[FromBody] ShopTakeMethodViewModel shopTakeMethod)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.SaveShopTakeMethod(id, _mapper.Map<ShopTakeMethodDto>(shopTakeMethod))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            var images = await _informationService.UploadImages(files);
            var result=images.Select(x => _mapper.Map<UploadImageViewModel>(x)).JsonSerialize();
            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = _mapper.Map<UploadImageViewModel>(await _informationService.UploadImage(file)).JsonSerialize();
            return new JsonResult(result);
        }
        [Authorize]
        [HttpGet]
        public IActionResult BrandFacade()
        {
            var brandFacadeVM=_mapper.Map<BrandFacadeViewModel>(_informationService.GetBrandFacade()).JsonSerialize();
            return new JsonResult(brandFacadeVM);
        }
        [Authorize]
        [HttpPost]
        public IActionResult BrandFacade([FromBody] BrandFacadeViewModel brandFacade)
        {
            var resultVM= _mapper.Map<SaveViewModel>(_informationService.SaveBrandFacade(_mapper.Map<BrandFacadeDto>(brandFacade))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        public IActionResult BrandImages()
        {
            var brandImagesVM=_mapper.Map<BrandImagesViewModel>(_informationService.GetBrandImages()).JsonSerialize();
            return new JsonResult(brandImagesVM);
        }
        [Authorize]
        [HttpPost]
        public IActionResult BrandImages([FromBody] BrandImagesViewModel brandImages)
        {
            var resultVM= _mapper.Map<SaveViewModel>(_informationService.SaveBrandImages(_mapper.Map<BrandImagesDto>(brandImages))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        public IActionResult ShopImages()
        {
            var shopImagesVM=_mapper.Map<ShopImagesViewModel>(_informationService.GetShopImages()).JsonSerialize();
            return new JsonResult(shopImagesVM);
        }
        [Authorize]
        [HttpPost]
        public IActionResult ShopImages([FromBody] ShopImagesViewModel shopImages)
        {
            var resultVM= _mapper.Map<SaveViewModel>(_informationService.SaveShopImages(_mapper.Map<ShopImagesDto>(shopImages))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        public IActionResult BrandIntroduce()
        {
            var brandIntroduceVM = _mapper.Map<BrandIntroduceViewModel>(_informationService.GetBrandIntroduce()).JsonSerialize();
            return new JsonResult(brandIntroduceVM);
        }
        [Authorize]
        [HttpPost]
        public IActionResult BrandIntroduce([FromBody] BrandIntroduceViewModel brandIntroduce)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.SaveBrandIntroduce(_mapper.Map<BrandIntroduceDto>(brandIntroduce))).JsonSerialize();
            return new JsonResult(resultVM);
        }
        [Authorize]
        [HttpGet]
        public IActionResult BrandWeb()
        {
            var brandWebVM = _mapper.Map<BrandWebViewModel>(_informationService.GetBrandWeb()).JsonSerialize();
            return new JsonResult(brandWebVM);
        }
        [Authorize]
        [HttpPost]
        public IActionResult BrandWeb([FromBody]BrandWebViewModel brandWeb)
        {
            var resultVM = _mapper.Map<SaveViewModel>(_informationService.SaveBrandWeb(_mapper.Map<BrandWebDto>(brandWeb))).JsonSerialize();
            return new JsonResult(resultVM);
        }
    }
}
