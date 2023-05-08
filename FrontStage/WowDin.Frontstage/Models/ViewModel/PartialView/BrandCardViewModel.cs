using System.Collections.Generic;

namespace WowDin.Frontstage.Models.ViewModel.PartialView
{
    public class BrandCardViewModel
    {
        public IEnumerable<BrandCardDataList> BrandCardList { get; set; }

        public class BrandCardDataList
        {
            public int BrandId { get; set; }
            public string CardImgUrl { get; set; }
            public string BrandLogo { get; set; }
            public string BrandName { get; set; }
            public string Srar { get; set; }
            public List<Category> Categories { get; set; }
            public string BrandSlogan { get; set; }
            public class Category
            {
                public string CategoryFig { get; set; }
                public string CategoryName { get; set; }
            }
        }
    }
}
