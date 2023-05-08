using System;
using System.Collections.Generic;
using WowDin.Frontstage.Models.Dto.Activity;
using WowDin.Frontstage.Models.ViewModel.Store;

namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class CartDetailViewModel
    {
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }
        public int Cartid { get; set; }
        public bool IsGroupBuy { get; set; }
        public bool IsCash { get; set; }
        public string TakeMethod { get; set; }
        public string StoreAddress { get; set; } //預設店家自取
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }
        public CartShopDataVM CartShopData { get; set; }
        public class CartShopDataVM
        {
            public string Brand { get; set; }
            public string BrandImg { get; set; }
            public string ShopName { get; set; }
            public string ShopPhone { get; set; }
            public string ShopPaymentType { get; set; }
            public string ShopTakeMethodType { get; set; }
        }
        public string OpenTime { get; set; } //判斷取餐時間選單
        public string CloseTime { get; set; } //判斷取餐時間選單
        public string OpenDayList { get; set; } // 判斷日期是否可以訂餐
        //public DateTime PickUpDate { get; set; }
        //public DateTime PickUpTime { get; set; }

        public IEnumerable<ProductDetailsByCartVM> ProductDetailsByCart { get; set; }
        public string ProductDetailsByCartJson { get; set; }

        public class ProductDetailsByCartVM
        {
            public int UserAcountId { get; set; }
            public string NickName { get; set; }
            public string Photo { get; set; }
            public IEnumerable<ProductDetailsByUserVM> ProductDetailsByUserData { get; set; }
            public class ProductDetailsByUserVM
            {
                public string NickName { get; set; } //UserName
                public string ProductName { get; set; }
                public decimal UnitPrice { get; set; }
                public decimal Discount { get; set; }
                public int Quantity { get; set; }
                public string Note { get; set; }
                public decimal Price { get; set; }
            }
        }


        public string Products { get; set; }
        public ProductDetailModal ProductDetailModal { get; set; } //from store
        public string Discount { get; set; }

        //數量&價格加總
        public string TotalPrice { get; set; }
        public string TotalQuantity { get; set; }
        public string UserName { get; set; } //主揪
        public string Phone { get; set; } //主揪連絡電話
        public string Message { get; set; } //給店家留言
        public string PaymentType { get; set; } 
        public string VATnumber { get; set; }
        public string Shop { get; set; }
        public string Brand { get; set; }
        public string OrderStamp { get; set; }

        public IEnumerable<UsableCouponByUserVM> UsableCoupons { get; set; }

        public class UsableCouponByUserVM
        {
            public int CouponId { get; set; }
            public string CouponTitle { get; set; }
        }
    }
}
