namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class AddCartDetailsOutputDto
    {
        public AddCartDetailsOutputDto() 
        {
            Order = new CartDetailToOrderData();
        }
        public bool IsSuccess { get; set; }
        public CartDetailToOrderData Order { get; set; }
        public class CartDetailToOrderData 
        {
            public int OrderId { get; set; }
            public int UserAccountId { get; set; }
            public int ShopId { get; set; }
            public string OrderDate { get; set; } //訂單成立時間
            public string PickUpTime { get; set; }
            public string UpdateTime { get; set; } //編輯時間
            public string OrderStamp { get; set; } //用時間戳記產訂單號
            public string Takemethod { get; set; }
            public string Message { get; set; }
            public int OrderState { get; set; }
            public string PaymentType { get; set; }
            public int PayState { get; set; }
            public string ReceiptType { get; set; }
            public string DeliveryAddress { get; set; }
            public string VATNumber { get; set; }
        }
       
        
    }
}
