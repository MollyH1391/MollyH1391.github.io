using System.Collections.Generic;

namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class CartsViewModel
    {
        public IEnumerable<CartDataViewModel> CartListByUserVM { get; set; }
        public bool IsCartExist { get; set; }
        public string CartListByUserJson { get; set; }

        public class CartDataViewModel
        {
            public int CartId { get; set; }
            public string OrderDate { get; set; } //購物車建立時間
            public CartShopDataViewModel ShopDataVM { get; set; }
            public CartDetailDataDViewModel CartDetailDataVM { get; set; }
            

            public class CartShopDataViewModel
            {
                public string Brand { get; set; }
                public string BrandImg { get; set; }
                public int ShopId { get; set; }
                public string ShopName { get; set; }
            }
            public class CartDetailDataDViewModel
            {
                public IEnumerable<CartDetailIdVM> CartDetailIds { get; set; }
                public bool IsGroupBuy { get; set; }
                public string TotalPriceByCart { get; set; }
                public string TotalQuantityByCart { get; set; }
            }
            public class CartDetailIdVM 
            {
                public int cartDetailId { get; set; }
            }
        }

    }
}
