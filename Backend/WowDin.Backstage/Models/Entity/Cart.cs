using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Cart
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        public int CartId { get; set; }
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }
        public DateTime Orderdate { get; set; }
        public string Message { get; set; }
        public string GroupCode { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
