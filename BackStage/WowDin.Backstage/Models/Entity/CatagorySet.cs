using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class CatagorySet
    {
        public int CatagorySetId { get; set; }
        public int CatagoryId { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Catagory Catagory { get; set; }
    }
}
