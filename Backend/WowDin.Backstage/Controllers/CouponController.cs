using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WowDin.Backstage.Controllers
{
    public class CouponController : Controller
    {
        [Authorize]
        public IActionResult Coupon()
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

            ViewData["BaseUrl"] = baseUrl;
            ViewData["BrandId"] = int.Parse(User.Identity.Name); 

            return View("Coupon_V2");
        }

        [Authorize]
        public IActionResult Advertise()
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

            ViewData["BaseUrl"] = baseUrl;
            ViewData["BrandId"] = int.Parse(User.Identity.Name);

            return View();
        }
    }
}
