using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class ProductHistory
    {
        public int ProductHistoryId { get; set; }
        public int ProductId { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateContent { get; set; }

        public virtual Product Product { get; set; }
    }
}
