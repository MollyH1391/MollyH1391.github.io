using System;
using System.Collections.Generic;

namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class AddCartViewModel
    {
        public int MainAccountId { get; set; }
        public int ShopId { get; set; }
        public CartDetail CartDetails { get; set; }

        public class CartDetail
        {
            //public int OrderAccountId { get; set; }
            public string NickName { get; set; }
            public int ProductId { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quentity { get; set; }
            public string Specs { get; set; }
        }
    }
}
