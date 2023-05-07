using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Coupon
{
    public class ShopsForCouponDto
    {
        public IEnumerable<ShopAndProductsDto> Shops { get; set; }
    }

    public class ShopAndProductsDto
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public IEnumerable<ProductOfShopDto> Products { get; set; }
    }

    public class ProductOfShopDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
