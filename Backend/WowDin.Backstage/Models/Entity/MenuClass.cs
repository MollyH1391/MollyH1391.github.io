using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class MenuClass
    {
        public MenuClass()
        {
            Products = new HashSet<Product>();
        }

        public int MenuClassId { get; set; }
        public string ClassName { get; set; }
        public int ShopId { get; set; }
        public int? Sort { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
