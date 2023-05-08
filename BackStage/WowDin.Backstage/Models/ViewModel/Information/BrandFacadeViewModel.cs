using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class BrandFacadeViewModel
    {
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Slogan { get; set; }
        public string CardImgUrl { get; set; }
        public List<BrandManagementViewModel.Category> BrandCategories { get; set; }

    }
}
