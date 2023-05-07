using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class CreateOrderViewModel
    {
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }
        public int CartId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Pickupyear { get; set; }
        public int Pickupmonth { get; set; }
        public int Pickupday { get; set; }
        public int Pickuphour { get; set; }
        public int Pickupmin { get; set; }
        public string OrderStamp { get; set; } //用時間戳記產訂單號
        public string TakeMethod { get; set; }
        public string Message { get; set; }
        public string OrderState { get; set; }
        public string PaymentType { get; set; }
        public string PayState { get; set; }
        public int? CouponId { get; set; }
        public string ReceiptType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string VATNumber { get; set; }
        public decimal? DeliveryFee { get; set; }
        public string FinalPrice { get; set; }
    }
}
