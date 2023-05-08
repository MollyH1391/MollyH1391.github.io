using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CostomSet
    {
        public CostomSet()
        {
            CostomToSets = new HashSet<CostomToSet>();
        }

        public int CostomSetId { get; set; }
        public int ShopId { get; set; }
        public string SetName { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<CostomToSet> CostomToSets { get; set; }
    }
}
