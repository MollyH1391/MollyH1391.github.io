namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class ECpayViewModel
    {
        public int OrderId { get; set; }
        public string PaymentDate { get; set; }
        public string OrderStamp { get; set; }
        public string TakeMethod { get; set; }
        public string PickUpTime { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string BrandName { get; set; }
        public bool isCash { get; set; }
    }
}
