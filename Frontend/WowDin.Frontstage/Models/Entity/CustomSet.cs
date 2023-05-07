using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CustomSet
    {
        public CustomSet()
        {
            CustomToSets = new HashSet<CustomToSet>();
        }

        public int CustomSetId { get; set; }
        public int ShopId { get; set; }
        public string SetName { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<CustomToSet> CustomToSets { get; set; }
    }
}
