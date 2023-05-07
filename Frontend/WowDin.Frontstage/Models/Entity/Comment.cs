using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int UserAccountId { get; set; }
        public string Comment1 { get; set; }
        public int Star { get; set; }
        public DateTime Date { get; set; }
        public int ShopId { get; set; }
        public int OrderId { get; set; }
        public int BrandId { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
