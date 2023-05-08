using System.Collections.Generic;
using WowDin.Frontstage.Models.Dto.PartialView;

namespace WowDin.Frontstage.Models.Dto.Store
{
    public class GetAllBrandDto
    {
        public IEnumerable<BrandCardDto> BrandCardList { get; set; }
        public IEnumerable<AddressDto> AddressList { get; set; }
    }
    public class BrandCardDto
        {
            public int BrandId { get; set; }
            public string CardImgUrl { get; set; }
            public string BrandLogo { get; set; }
            public string BrandName { get; set; }
            public double BrandStar { get; set; }
            public int BrandStarAmount { get; set; }
            public List<Category> Categories { get; set; }
            public string BrandSlogan { get; set; }
            public class Category
            {
                public string CategoryFig { get; set; }
                public string CategoryName { get; set; }
            }
        
        }

    public class AddressDto
    {
        public int UserAccountId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }
}
