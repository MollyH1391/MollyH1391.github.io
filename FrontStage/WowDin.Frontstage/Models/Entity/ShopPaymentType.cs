using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class ShopPaymentType
    {
        public int ShopPaymentTypeId { get; set; }
        public int ShopId { get; set; }
        public int PaymentType { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
