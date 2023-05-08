namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class SaveViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string UpdateTime { get; set; }
        public Shop NewShop { get; set; }
        public class Shop
        {
            public int value { get; set; }
            public string text { get; set; }
        }


    }
}
