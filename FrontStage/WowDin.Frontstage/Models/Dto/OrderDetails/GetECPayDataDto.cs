namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class GetECPayDataDto
    {
        public int ShopId { get; set; }
        public string BrandName { get; set; }
        public string ShopName { get; set; }
        public string CartId { get; set; }
        public string TotalAmount { get; set; }
        public string OrderStamp { get; set; }
        public string TakeMethod { get; set; }
        public string PickUpTime { get; set; }
    }
}
