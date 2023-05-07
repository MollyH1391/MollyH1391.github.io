using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using WowDin.Frontstage.Common;

namespace WowDin.Frontstage.Controllers
{
    public class JsonDataController : Controller
    {
        public IActionResult GetCityDistrict()
        {
            var json = System.IO.File.ReadAllText(@"wwwroot/json/CityDistrict.json");

            return Ok(json);
        }
    }
}
