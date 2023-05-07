using System;
using System.Collections.Generic;
using WowDin.Frontstage.Models.Dto.Store;
using static WowDin.Frontstage.Common.ModelEnum.ShopMethodEnum;
using static WowDin.Frontstage.Common.ModelEnum.ShopPaymentTypeEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class GetCartDetailsDto
    {
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }
        public int Cartid { get; set; }
        public bool IsCash { get; set; }
        public string UserName { get; set; } //主揪
        public string Phone { get; set; } //主揪連絡電話
        public bool IsGroupBuy { get; set; }
        public CartShopDataDto ShopData { get; set; }
        public class CartShopDataDto
        {
            public string Brand { get; set; }
            public string BrandImg { get; set; }
            public string ShopName { get; set; }
            public string ShopPhone { get; set; }
            public string ShopPaymentType { get; set; }
            public string ShopTakeMethodType { get; set; }
        }

        public string StoreAddress { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }
        public DateTime OpenTime { get; set; } //判斷取餐時間選單
        public DateTime CloseTime { get; set; } //判斷取餐時間選單
        public string OpenDayList { get; set; } // 判斷日期是否可以訂餐

        public IEnumerable<ProductDetailsByCartDto> ProductDetailsByCart { get; set; }
        public class ProductDetailsByCartDto 
        {
            public int UserAcountId { get; set; }
            public string NickName { get; set; }
            public string Photo { get; set; }

            public IEnumerable<ProductDetailsByUserDto> ProductDetailsByUser { get; set; }
            public class ProductDetailsByUserDto
            {
                public int CartDetailId { get; set; }
                public string NickName { get; set; } //UserName
                public string ProductName { get; set; }
                public decimal UnitPrice { get; set; }
                public decimal Discount { get; set; }
                public int Quantity { get; set; }
                public string Note { get; set; }
                public decimal Price { get; set; }
                public ProductDto Product { get; set; }
            }
        }

        

        
        public bool HasSticker { get; set; }

        

        //數量&價格加總
        public decimal TotalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string OrderStamp { get; set; }
        public string DiscountMessage { get; set; }
        public decimal Discount { get; set; }
        public int CouponId { get; set; }

        
    }
}

