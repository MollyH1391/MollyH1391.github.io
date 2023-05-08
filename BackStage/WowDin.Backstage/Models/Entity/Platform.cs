using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Platform
    {
        public Platform()
        {
            Websites = new HashSet<Website>();
        }

        public int PlatformId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<Website> Websites { get; set; }
    }
}
