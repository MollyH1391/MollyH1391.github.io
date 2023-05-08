using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Catagory
    {
        public Catagory()
        {
            CatagorySets = new HashSet<CatagorySet>();
        }

        public int CatagoryId { get; set; }
        public string Name { get; set; }
        public string Fig { get; set; }

        public virtual ICollection<CatagorySet> CatagorySets { get; set; }
    }
}
