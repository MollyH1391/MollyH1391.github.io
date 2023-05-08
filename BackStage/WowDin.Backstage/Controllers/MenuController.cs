using Microsoft.AspNetCore.Mvc;

namespace WowDin.Backstage.Controllers
{
    public class MenuController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        //店家用
        //public IActionResult MenuManage()
        //{
        //    return View();
        //}

        //品牌用
        public IActionResult MenuManage()
        {
            //傳入URL
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

            ViewData["BaseUrl"] = baseUrl;
            ViewData["BrandId"] = int.Parse(User.Identity.Name); 

            return View();
        }

    }
}
