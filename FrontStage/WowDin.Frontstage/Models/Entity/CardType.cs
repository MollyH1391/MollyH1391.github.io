using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class CardType
    {
        public int CardTypeId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int CardLevel { get; set; }
        public int? Range { get; set; }
        public string CardImgUrl { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
