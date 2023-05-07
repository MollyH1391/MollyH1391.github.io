using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class Costom
    {
        public Costom()
        {
            CostomSelections = new HashSet<CostomSelection>();
        }

        public int CostomId { get; set; }
        public string Name { get; set; }
        public int? MaxAmount { get; set; }
        public bool Necessary { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<CostomSelection> CostomSelections { get; set; }
    }
}
