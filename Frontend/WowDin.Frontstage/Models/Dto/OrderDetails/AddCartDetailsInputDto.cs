using System;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class AddCartDetailsInputDto
    {
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }
        public int CartId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickUpTime { get; set; }

        public DateTime UpdateDate { get; set; }
        public string OrderStamp { get; set; } //用時間戳記產訂單號
        public TakeMethodEnum TakeMethodId { get; set; }
        public string Message { get; set; }
        public OrderStateEnum OrderState { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public PayStateEnum PayState { get; set; }
        public int? CouponId { get; set; }
        public ReceiptTypeEnum ReceiptType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string VATNumber { get; set; }
        public string FinalPrice { get; set; }

        public decimal? DeliveryFee { get; set; }

    }
}
