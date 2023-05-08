using Microsoft.AspNetCore.Mvc;
using System;
using WowDin.Backstage.Models.Base;
using WowDin.Backstage.Models.ViewModel.Order;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Controllers
{
   
    public class OrderController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Order/OrderDetailsByShop/{id}")]
        public IActionResult OrderDetailsByShop(int id)
        {
            //傳入shopId
            var shopId = id;
            ViewData["ShopId"] = shopId; // 連接到shop訂單

            return View();
        }

        public IActionResult OrderDetailsByBrand()
        {
            //ViewData["BrandId"] = 1; // by 品牌登入資料
            ViewData["BrandId"] = int.Parse(User.Identity.Name);

            return View();
        }

                
    }
}
