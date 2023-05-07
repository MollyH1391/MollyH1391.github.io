using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Favorite
    {
        public int FavoraiteId { get; set; }
        public int UserAccountId { get; set; }
        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
