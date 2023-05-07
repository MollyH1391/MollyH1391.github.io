using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class ShopFigure
    {
        public int ShopFigureId { get; set; }
        public int BrandId { get; set; }
        public string Path { get; set; }
        public string AltText { get; set; }
        public int? Sort { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
