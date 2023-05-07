using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WowDin.Backstage.Controllers
{
    public class MessageHandlerController : Controller
    {
        [TempData]
        public string Message { get; set; }
        public IActionResult MessagePage()
        {
            if (!TempData.ContainsKey("Message"))
            {
                return new EmptyResult();
            }
            TempData.Keep();
            return View();
        }
    }
}
