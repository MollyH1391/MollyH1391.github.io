using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CartDetail
    {
        public int CartId { get; set; }
        public int UserAccountId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public int CartDetailId { get; set; }
        public decimal UnitPrice { get; set; }
        public string NickName { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
