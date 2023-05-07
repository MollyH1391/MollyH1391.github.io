using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Order
{
    public class GetAllOrderDetailsByBrandVM
    {
       
        public int OrderId { get; set; }
        
        public int ShopId { get; set; }
        public string UserName { get; set; }
        public string OrderDate { get; set; }
        public string PickUpTime { get; set; }
        public string ShopName { get; set; }
        public string BrandName { get; set; }
        public string TakeMethod { get; set; }
        public string Message { get; set; }
        public string OrderState { get; set; }
        public int? CouponId { get; set; }
        public string ReceiptType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string UpdateDate { get; set; }
        public int? UsePoint { get; set; }
        public string OrderStamp { get; set; }
        public string Vatnumber { get; set; }
        public string Coupon { get; set; }

        public string TotalPrice { get; set; }
        public string TotalQuantity { get; set; }
        public IEnumerable<OrderDetails> OrderDetailsList { get; set; }

        public class OrderDetails
        {
            public int OrderDetailId { get; set; }
            public int OrderId { get; set; }
            public string UserName { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Discount { get; set; }
            public short Quantity { get; set; }
            public string Note { get; set; }
            public bool? IsPaid { get; set; }
            public string Price { get; set; }
        }
        
    }
    
}
