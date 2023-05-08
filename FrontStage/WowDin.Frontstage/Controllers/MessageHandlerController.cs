using Microsoft.AspNetCore.Mvc;

namespace WowDin.Frontstage.Controllers
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
