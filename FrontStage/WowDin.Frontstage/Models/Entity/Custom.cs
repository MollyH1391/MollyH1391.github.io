using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class Custom
    {
        public Custom()
        {
            CustomSelections = new HashSet<CustomSelection>();
        }

        public int CustomId { get; set; }
        public string Name { get; set; }
        public int? MaxAmount { get; set; }
        public bool Necessary { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<CustomSelection> CustomSelections { get; set; }
    }
}
