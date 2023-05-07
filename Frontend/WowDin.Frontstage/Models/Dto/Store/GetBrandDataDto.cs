using System.Collections.Generic;
using WowDin.Frontstage.Models.Dto.PartialView;

namespace WowDin.Frontstage.Models.Dto.Store
{
    public class GetBrandDataDto
    {
        public BrandData Brands { get; set; }
        public IEnumerable<SearchZoneDto> SearchZoneList { get; set; }
        public IEnumerable<WebsitebDataList> WebsiteList { get; set; }
    }
    public class BrandData
    {
        public string BrandName { get; set; }
        public double BrandStar { get; set; }
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
}
