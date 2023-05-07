using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models;
using WowDin.Frontstage.Models.ViewModel.Home;
using WowDin.Frontstage.Models.ViewModel.PartialView;
using WowDin.Frontstage.Services;
using WowDin.Frontstage.Services.Interface;
using static WowDin.Frontstage.Models.ViewModel.Home.IndexViewModel;

namespace WowDin.Frontstage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActivityService _activityService;
        private readonly IStoreService _storeService;

        public HomeController(ILogger<HomeController> logger, IActivityService activityService, IStoreService storeService)
        {
            _logger = logger;
            _activityService = activityService;
            _storeService = storeService;
        }

        public IActionResult Index()
        {
            var figures = _activityService.GetIndexFigures();
            var brandDataList = _storeService.GetAllBrand();
            var brandList = brandDataList.BrandCardList;

            var result = new IndexViewModel()
            {

                BigPictures = figures.BigPictures.Select(x => new Picture() { PictureFile = x.PictureFile, BrandId = x.BrandId, Code = x.Code }),
                Smallpictures = figures.SmallPictures.Select(x => new Picture() { PictureFile = x.PictureFile, BrandId = x.BrandId, Code = x.Code }),
                BrandLists = brandList.Select(x => new BrandList
                {
                    BrandId = x.BrandId,
                    Logo = x.BrandLogo,
                }),

            };

            return View(result);
        }

        public IActionResult Customer()
        {
            return View();
        }



        public IActionResult PrivacyTerm()
        {
            return View();
        }

        public IActionResult UserTerm()
        {
            return View();
        }

        public IActionResult CreditTerm()
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
