using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class BrandHistory
    {
        public int BrandHistoryId { get; set; }
        public int BrandId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateContent { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
