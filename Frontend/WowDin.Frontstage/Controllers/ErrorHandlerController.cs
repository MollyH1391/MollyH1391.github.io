using Microsoft.AspNetCore.Mvc;

namespace CoreMvc5_Pillars.Controllers
{
    public class ErrorHandlerController : Controller
    {
        public IActionResult ErrorMessage()
        {
            if (!TempData.ContainsKey("ErrorMessage"))
            {
                return new EmptyResult();
            }

            TempData.Keep("ErrorMessage"); 

            return View();
        }
    }
}
