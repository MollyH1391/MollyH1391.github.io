using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Coupon
{
    public class ShopsForCouponVM
    {
        public IEnumerable<ShopAndProductsVM> Shops { get; set; }
    }

    public class ShopAndProductsVM
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public IEnumerable<ProductOfShopDto> Products { get; set; }
    }

    public class ProductOfShopVM
    {
        public int ProductId { get; set; }
        public int ProductName { get; set; }
    }
}
