using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CuponContainer
    {
        public int CuponContainerId { get; set; }
        public int UserAccountId { get; set; }
        public int CuponId { get; set; }
        public int CuponState { get; set; }

        public virtual Cupon Cupon { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
