using System;
using System.Collections.Generic;
using System.Linq;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.ViewModel
{
    public class OrderDetailsViewModel
    {
       
        public string OrderStamp { get; set; }
        public ShopData Shop { get; set; }
        public IEnumerable<OrderDetailData> OrderDetails { get; set; }
        public OrderData Order { get; set; }
        public CustomerData Customer { get; set; }
        public CommentDataVM Comments { get; set; }
        public int OrderId { get; set; }
        public string QuantityTotal { get; set; } //一個訂單的餐點數量加總
        public string PriceTotal { get; set; } //一個訂單的價格加總
        public string FinalPrice { get; set; } //扣除折扣後的應付金額
        public bool isCommented { get; set; }
        public bool orderIsComplete { get; set; }
        public string Discount { get; set; }
        

        //店家(店名、分店、電話、地址)
        public class ShopData
        {
            public int ShopId { get; set; }
            public string ShopName { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Path { get; set; } //品牌圖片
            public string Brand { get; set; }
        }


        public class OrderDetailData
        {
            public int UserAcountId { get; set; }
            public string NickName { get; set; }
            //public string NickName { get; set; }
            public string Photo { get; set; }
            public IEnumerable<ProductData> ProductDataList { get; set; }
            public string TotalPriceByUser { get; set; }
            public string TotalQuantityByUser { get; set; }

        }

        public class ProductData
        {
            public string UnitPrice { get; set; } //單價:計算價格用
            public string Discount { get; set; } //折扣:計算價格用
            public bool? IsPaid { get; set; } //團員是否已付款
            public string Quantity { get; set; } //訂單商品數量
            public string ProductName { get; set; }
            public string Note { get; set; }
        }


        public class OrderData
        {
            
            public string TakeMethod { get; set; }//取餐方式
            //public string TakeMethodId { get; set; }
            public DateTime OrderDate { get; set; }//訂單建立時間
            public DateTime PickUpTime { get; set; } //預計取餐時間
            public string ReceiptType { get; set; }
            //public string Address { get; set; }
            public string Message { get; set; }
            public string OrderStateEnum { get; set; }//訂單狀態(enum)
            public string PaymentType { get; set; }
            public string PayDate { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Address { get; set; }
            public bool isDelivery { get; set; }
            public string DeliveryFee { get; set; }
            public string Coupon { get; set; }
            public string VatNum { get; set; }
        }

        public class CustomerData
        {
            public string NickName { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }

        }

        public class CommentDataVM
        {
            public int Star { get; set; }
            public string Comment1 { get; set; }
        }
    }
}
