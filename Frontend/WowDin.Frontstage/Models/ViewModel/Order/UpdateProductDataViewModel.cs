namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class UpdateProductDataViewModel
    {
        public CartDetail CartDetails { get; set; }

        public class CartDetail
        {
            public string NickName { get; set; }
            public int ProductId { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quentity { get; set; }
            public string Specs { get; set; }
        }
    }
}
