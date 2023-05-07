using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Response
    {
        public int ResponseId { get; set; }
        public int UserAccountId { get; set; }
        public int? BrandId { get; set; }
        public int? ShopId { get; set; }
        public string ResponseContent { get; set; }
        public DateTime Date { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
