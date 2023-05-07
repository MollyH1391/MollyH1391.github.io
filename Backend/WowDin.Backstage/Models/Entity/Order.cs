using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PointHistories = new HashSet<PointHistory>();
        }

        public int OrderId { get; set; }
        public int UserAcountId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public int ShopId { get; set; }
        public int TakeMethodId { get; set; }
        public string Message { get; set; }
        public int OrderState { get; set; }
        public int? CouponId { get; set; }
        public int PaymentType { get; set; }
        public DateTime? PayDate { get; set; }
        public int PayState { get; set; }
        public int ReceiptType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? UsePoint { get; set; }
        public string OrderStamp { get; set; }
        public string Vatnumber { get; set; }
        public decimal? DeliveryFee { get; set; }
        public decimal? Discount { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual UserAccount UserAcount { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PointHistory> PointHistories { get; set; }
    }
}
