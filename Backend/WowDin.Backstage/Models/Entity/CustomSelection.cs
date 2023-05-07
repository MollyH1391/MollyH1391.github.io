using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class CustomSelection
    {
        public int CustomSelectionId { get; set; }
        public int CustomId { get; set; }
        public string Name { get; set; }
        public decimal AddPrice { get; set; }

        public virtual Custom Custom { get; set; }
    }
}
