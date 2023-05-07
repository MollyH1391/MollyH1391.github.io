using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CostomSelection
    {
        public int CostomSelectionId { get; set; }
        public int CostomId { get; set; }
        public string Name { get; set; }
        public decimal AddPrice { get; set; }

        public virtual Costom Costom { get; set; }
    }
}
