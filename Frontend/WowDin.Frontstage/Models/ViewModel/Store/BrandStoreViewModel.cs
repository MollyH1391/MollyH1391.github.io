using System.Collections.Generic;
using WowDin.Frontstage.Models.ViewModel.PartialView;


namespace WowDin.Frontstage.Models.ViewModel.Store
{
    public class BrandStoreViewModel
    { 
        public BrandData Brands { get; set; }
        public IEnumerable<SearchZoneViewModel> SearchZoneList { get; set; }
        public IEnumerable<WebsitebDataList> WebsiteList { get; set; }
        public IEnumerable<AddressDataList> AddressList { get; set; }

        public class BrandData
        {
            public string BrandName { get; set; }
            public string BrandStar { get; set; }
            public int BrandStarAmount { get; set; }
            public string Description { get; set; }
            public string Video { get; set; }
        }
        public class WebsitebDataList
        {
            public int PlatformId { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public string MediaLogo { get; set; }
            public string Webpic { get; set; }
        }
        public class AddressDataList
        {
            public int UserAccountId { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Address { get; set; }
        }
    }
}
