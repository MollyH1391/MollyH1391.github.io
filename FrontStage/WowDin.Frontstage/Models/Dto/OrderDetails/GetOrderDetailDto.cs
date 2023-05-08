using System;
using System.Collections.Generic;
using System.Linq;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class GetOrderDetailDto
    {

        public int OrderId { get; set; }
        public string OrderStamp { get; set; }
        public UserData User { get; set; }
        public ShopData Shop { get; set; }
        public IEnumerable<OrderDetailData> OrderDetails { get; set; }
        public OrderData Order { get; set; }
        public CommentDataDto CommentData { get; set; }
        public int QuantityTotal { get; set; } //一個訂單的餐點數量加總
        public decimal PriceTotal { get; set; } //一個訂單的價格加總
        public string FinalPrice { get; set; } //扣除折扣後的應付金額
        public bool isCommented { get; set; }
        public bool orderIsComplete { get; set; }
        public decimal Discount { get; set; }
    }

    public class UserData
    {
        public int UserAcountId { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
    }
    public class ShopData
    {
        
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Path { get; set; }
        public string Brand { get; set; }
    }
    public class OrderDetailData
    {
        public int UserAcountId { get; set; }
        public string NickName { get; set; }
        public string Photo { get; set; }
        public IEnumerable<ProductData> ProductDataList { get; set; }
        public decimal TotalPriceByUser { get; set; }
        public int TotalQuantityByUser { get; set; }

    }

    public class ProductData 
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public short Quantity { get; set; }
        public string Note { get; set; }
        public bool? IsPaid { get; set; }
        public decimal Price { get; set; }
    }
    public class OrderData
    {
        public TakeMethodEnum TakeMethod { get;set; }
        //public int TakeMethodEnum { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickUpTime { get; set; } //取餐時間
        public string Message { get; set; }
        public OrderStateEnum OrderStateEnum { get; set; }
        public string Coupon { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public string PayDate { get; set; }
        public int PayState { get; set; }
        public ReceiptTypeEnum ReceiptType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? UsePoint { get; set; }
        public bool isDelivery { get; set; }
        public decimal DeliveryFee { get; set; }
        public string VatNum { get; set; }
    }
    public class CommentDataDto
    {
        public int Star { get; set; }
        public string Comment1 { get; set; }
    }
}
