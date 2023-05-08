using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WowDin.Backstage.Models.Base;
using WowDin.Backstage.Models.Dto.Order;
using WowDin.Backstage.Models.ViewModel.Order;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Controllers
{

    [ApiController]
    //[Route("api/[action]/{id}")]
    public class OrderApiController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        
        /// <summary>
        /// Get all OrderDetails by shopid (未接單: OrderEstablished)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/OrderDetails_Shop/{id}")]
        public IActionResult OrderDetails_Shop(int id)
        {
            //var shopid = 1;
            try
            {
                var result = _orderService.GetAllOrderDetailsByShop(id);
                return Ok(new APIResult(APIStatus.Success, string.Empty, result));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, null));
            }
        }
        

        /// <summary>
        /// 接受訂單  (已接單: OrderAccepted)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UpdateOrderState_Accept")]
        public IActionResult UpdateOrderState_Accept(GetAllOrderDetailsByShopVM query)
        {
            try
            {
                _orderService.UpdateOrderState_Accept(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        [HttpPut]
        [Route("api/UpdateOrderState_AcceptALL")]
        public IActionResult UpdateOrderState_AcceptALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            try
            {
                _orderService.UpdateOrderState_AcceptALL(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        [HttpPut]
        [Route("api/UpdateOrderState_CancelALL")]
        public IActionResult UpdateOrderState_CancelALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            try
            {
                _orderService.UpdateOrderState_CancelALL(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        [HttpPut]
        [Route("api/UpdateOrderState_CompletelALL")]
        public IActionResult UpdateOrderState_CompletelALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            try
            {
                _orderService.UpdateOrderState_CompleteALL(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        /// <summary>
        /// 取消訂單  (已接單: OrderRejected_CancelledByShop)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UpdateOrderState_Cancel")]
        public IActionResult UpdateOrderState_Cancel(GetAllOrderDetailsByShopVM query)
        {
            try
            {
                _orderService.UpdateOrderState_Cancel(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        /// <summary>
        /// 完成訂單  (已接單: OrderComplete)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UpdateOrderState_Complete")]
        public IActionResult UpdateOrderState_Complete(GetAllOrderDetailsByShopVM query)
        {
            try
            {
                _orderService.UpdateOrderState_Complete(query);
                return Ok(new APIResult(APIStatus.Success, string.Empty, true));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, false));
            }
        }

        [Route("api/OrderDetails_Brand")]
        [HttpGet]
        [Authorize]
        public IActionResult OrderDetails_Brand()
        {
            var id = int.Parse(User.Identity.Name);
            try
            {
                var result = _orderService.GetAllOrderDetailsByBrand(id);
                return Ok(new APIResult(APIStatus.Success, string.Empty, result));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, null));
            }
        }

        [Route("api/ShopListByBrand")]
        [HttpGet]
        [Authorize]
        public IActionResult ShopListByBrand()
        {
            
            var id = int.Parse(User.Identity.Name);
            try
            {
                var result = _orderService.GetShopsByBrand(id);
                return Ok(new APIResult(APIStatus.Success, string.Empty, result));
            }
            catch (Exception ex)
            {
                return Ok(new APIResult(APIStatus.Fail, ex.Message, null));
            }
        }

    }
}
