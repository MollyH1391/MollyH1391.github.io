using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Imformation
{
    public class GetBrandByIdDto
    {
        public BrandData Brands { get; set; }
        public IEnumerable<BrandDataList> BrandList { get; set; }
        public IEnumerable<WebsitebDataList> WebsiteList { get; set; }

        public class BrandData
        {
            public string BrandName { get; set; }
            public string Logo { get; set; }
            public string Slogen { get; set; }
            public string Description { get; set; }
            public string Video { get; set; }
            public string FirstColor { get; set; }
            public string SecondColor { get; set; }
            public double BrandStar { get; set; }
            public int BrandStarAmount { get; set; }
        }
        public class BrandDataList
        {
            public string BrandAdImgPath { get; set; }
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
}
