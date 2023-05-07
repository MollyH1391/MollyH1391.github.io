using System.Collections.Generic;

namespace WowDin.Frontstage.Models.Dto.Order
{
    public class GetAllDataDto
    {
        public IEnumerable<ShopListDto> Shops { get; set; }
        public IEnumerable<BrandListDto> Brands { get; set; }

    }  
    public class ShopListDto
    {
        public int BrandId { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
    }

    public class BrandListDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
