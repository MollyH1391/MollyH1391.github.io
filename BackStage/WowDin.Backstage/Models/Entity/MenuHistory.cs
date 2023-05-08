using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class MenuHistory
    {
        public int MenuHistoryId { get; set; }
        public int ShopId { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateContent { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
