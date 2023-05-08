using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.ViewModel.Information;
using WowDin.Backstage.Models.ViewModel.Menu;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;

namespace WowDin.Backstage.Controllers
{
    [ApiController]
    public class MenuApiController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;
        private readonly IInformationService _informationService;

        public MenuApiController(IMenuService menuService, IMapper mapper, IInformationService informationService)
        {
            _menuService = menuService;
            _mapper = mapper;
            _informationService = informationService;
        }

        [Authorize]
        [Route("api/GetShops/{id}")]
        [HttpGet]
        public IActionResult GetShops(int id)
        {
            var result = _menuService.GetShopsOfBrand(id);

            return Ok(result);
        }

        [Authorize]
        [Route("api/MenuData/{id}")]
        [HttpGet]
        public IActionResult GetMenuData(int id)
        {
            var result = _menuService.GetMenuData(id);

            var menuVM = _mapper.Map<MenuVM>(result.Result);
            menuVM.UpdateTime = _menuService.GetRecord(id).ToString("yyyy/MM/dd HH:mm:ss");
            result.Result = menuVM;
            
            return Ok(result);
        }

        [Authorize]
        [Route("api/Copy")]
        [HttpPost]
        public IActionResult CopyMenu([FromBody]CopyMenuInputVM request)
        {
            var result = _menuService.CopyMenu(request);

            return Ok(result);
        }

        [Authorize]
        [Route("api/CopyCustom")]
        [HttpPost]
        public IActionResult CopyCustom([FromBody] CopyCustomInputVM request)
        {
            var result = _menuService.CopyCustom(request);

            return Ok(result);
        }

        [Authorize]
        [Route("api/Arrange")]
        [HttpPost]
        public IActionResult SaveArrangement([FromBody] ArrangementInputVM request)
        {
            var result = _menuService.SaveArrangement(request);

            return Ok(result);
        }

        #region Class

        [Authorize]
        [Route("api/Class")]
        [HttpPost]
        public IActionResult CreateClass([FromBody] ClassInputVM classInput)
        {
            var result = _menuService.CreateClass(classInput);
            
            return Ok(result);
        }

        [Authorize]
        [Route("api/Class")]
        [HttpDelete]
        public IActionResult DeleteClass([FromBody] ClassInputVM classInput)
        {
            var result = _menuService.DeleteClass(classInput);

            return Ok(result);
        }

        [Authorize]
        [Route("api/Class")]
        [HttpPut]
        public IActionResult EditClass([FromBody] ClassInputVM classInput)
        {
            var result = _menuService.EditClass(classInput);

            return Ok(result);
        }
        #endregion

        #region Product
        [Authorize]
        [Route("api/Product")]
        [HttpPost]
        public IActionResult UpdateProduct([FromBody] ProductInputVM productInput)
        {
            APIResult result;

            if (productInput.Id.Contains("new"))
            {
                result = _menuService.CreateProduct(productInput);
            }
            else
            {
                result = _menuService.EditProduct(productInput);
            }

            return Ok(result);
        }

        [Authorize]
        [Route("api/Product")]
        [HttpPut]
        public IActionResult TempDeleteProduct([FromBody] ProductDeleteInputVM productInput)
        {
            var result = _menuService.TempDeleteProduct(productInput);

            return Ok(result);
        }

        [Authorize]
        [Route("api/Product")]
        [HttpDelete]
        public IActionResult DeleteProduct([FromBody] ProductDeleteInputVM productInput)
        {
            var result = _menuService.DeleteProduct(productInput);

            return Ok(result);
        }

        #endregion
    }
}
