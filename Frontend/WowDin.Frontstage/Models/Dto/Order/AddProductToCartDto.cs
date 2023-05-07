using System;
using System.Collections.Generic;

namespace WowDin.Frontstage.Models.Dto.Order
{
    public class AddProductToCartDto
    {
        public int LeaderId { get; set; }
        public int ShopId { get; set; }
        public DateTime OrderTime { get; set; }
        public CartDetailDto CartDetails { get; set; }

    }
    public class CartDetailDto
    {
        public int OrderAccountId { get; set; }
        public string NickName { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quentity { get; set; }
        public string Specs { get; set; }
    }
}
