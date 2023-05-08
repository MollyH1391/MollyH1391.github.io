using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class ShopHistory
    {
        public int ShopHistoryId { get; set; }
        public int ShopId { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Updator { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateContent { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
