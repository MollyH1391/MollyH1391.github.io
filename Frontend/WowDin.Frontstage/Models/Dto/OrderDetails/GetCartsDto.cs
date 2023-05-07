using System.Collections.Generic;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class GetCartsDto
    {
        public IEnumerable<CartDataDto> CartListByUser { get; set; }
        public bool IsCartExist { get; set; }

        public class CartDataDto
        {
            public int CartId { get; set; }
            public string OrderDate { get; set; } //購物車建立時間
            public CartShopDataDto ShopData { get; set; }
            public CartDetailDataDto CartDetailData { get; set; }

            public class CartShopDataDto 
            {
                public string Brand { get; set; }
                public string BrandImg { get; set; }
                public int ShopId { get; set; }
                public string ShopName { get; set; }
            }

            public class CartDetailDataDto
            {
                public IEnumerable<CartDetailIdDto> CartDetailIds { get; set; }
                public bool IsGroupBuy { get; set; } //用group code 判斷
                public decimal TotalPriceByCart { get; set; }
                public int TotalQuantityByCart { get; set; }
            }

            public class CartDetailIdDto
            {
                public int cartDetailId { get; set; }
            }

        }
    }
}
