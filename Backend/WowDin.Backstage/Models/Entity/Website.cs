using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Website
    {
        public int WebsiteId { get; set; }
        public int BrandId { get; set; }
        public int PlatformId { get; set; }
        public string Path { get; set; }
        public string Webpic { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
