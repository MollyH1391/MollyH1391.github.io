using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int UserAccountId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public short Quantity { get; set; }
        public string Note { get; set; }
        public bool? IsPaid { get; set; }

        public virtual Order Order { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
