using System.Collections.Generic;
using WowDin.Frontstage.Models.ViewModel.PartialView;

namespace WowDin.Frontstage.Models.ViewModel.Store
{
    public class SearchViewModel
    {
        public string Method { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string SelectedBrand { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedEvaluate { get; set; }
        public List<string> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<ShopCardViewModel> ShopCards { get; set; }

        public class Category
        {
            public string Name { get; set; }
            public string Img { get; set; }
        }
    }
}
