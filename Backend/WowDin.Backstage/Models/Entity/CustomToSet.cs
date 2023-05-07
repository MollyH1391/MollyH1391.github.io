using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class CustomToSet
    {
        public int CustomToSetId { get; set; }
        public int CustomId { get; set; }
        public int CustomSetId { get; set; }

        public virtual CustomSet CustomSet { get; set; }
    }
}
