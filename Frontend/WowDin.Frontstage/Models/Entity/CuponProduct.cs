using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CuponProduct
    {
        public int CuponId { get; set; }
        public int ProductId { get; set; }

        public virtual Cupon Cupon { get; set; }
        public virtual Product Product { get; set; }
    }
}
