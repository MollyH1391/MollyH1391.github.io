using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CostomToSet
    {
        public int CostomToSetId { get; set; }
        public int CostomId { get; set; }
        public int CostomSetId { get; set; }

        public virtual CostomSet CostomSet { get; set; }
    }
}
