using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WowDin.Backstage.Models.ViewModel.Member;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Controllers
{
    public class MemberController : Controller
    {
        [Authorize]
        public IActionResult MemberPoint()
        {
            return View();
        }

        [Authorize]
        public IActionResult MemberCardGrading()
        {
            return View();
        }

    }
}
