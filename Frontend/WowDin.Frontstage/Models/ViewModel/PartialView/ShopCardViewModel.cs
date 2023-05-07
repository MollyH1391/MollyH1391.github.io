using System.Collections.Generic;

namespace WowDin.Frontstage.Models.ViewModel.PartialView
{
    public class ShopCardViewModel
    {
        public int ShopId { get; set; }
        public string ShopFig { get; set; }
        public string ShopName { get; set; }
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string Distance { get; set; }
        public string Star { get; set; }
        public string Address { get; set; }
        public List<Category> Categories { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsFavorite { get; set; }

        public class Category
        {
            public string CategoryFig { get; set; }
            public string CategoryName { get; set; }
        }
    }
}
