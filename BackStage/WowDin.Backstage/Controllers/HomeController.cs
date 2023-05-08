using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.ViewModel;
using WowDin.Backstage.Models.ViewModel.Home;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using static WowDin.Backstage.Models.ViewModel.Home.IndexViewModel;

namespace WowDin.Backstage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberService _memberService;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IMemberService memberService, IHomeService homeService)
        {
            _logger = logger;
            _memberService = memberService;
            _homeService = homeService;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Comment()

        {
            int brandId = int.Parse(User.Identity.Name);
            var result = _homeService.GetCommentData(brandId).Select(x => new CommentViewModel
            {
                BrandName = x.BrandName,
                ShopName = x.ShopName,
                Star = x.Star,
                Comment = x.Comment,
                UserName = x.UserName,
                Date = x.Date
            });

            return View(result);
        }
  

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
