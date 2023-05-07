namespace WowDin.Frontstage.Models.Dto.Order
{
    public class GetCartCompleteDto
    {
        public int OrderId { get; set; }
        public string OrderStamp { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string BrandName { get; set; }
        public string TakeMethod { get; set; }
        public string PickUpTime { get; set; }
        public string CompleteMessage { get; set; }
        public bool isCash { get; set; }
    }
}
