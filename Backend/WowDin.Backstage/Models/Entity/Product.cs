using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Product
    {
        public Product()
        {
            CouponProducts = new HashSet<CouponProduct>();
            Customs = new HashSet<Custom>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public int MenuClassId { get; set; }
        public int State { get; set; }
        public string Fig { get; set; }
        public decimal BasicPrice { get; set; }
        public string Note { get; set; }
        public int? Sort { get; set; }

        public virtual MenuClass MenuClass { get; set; }
        public virtual ICollection<CouponProduct> CouponProducts { get; set; }
        public virtual ICollection<Custom> Customs { get; set; }
    }
}
