using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowDin.Backstage.Models.ViewModel
{
    public class CommentViewModel
    {
        public string BrandName { get; set; }
        public string ShopName { get; set; }
        public int Star { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime  Date { get; set; }
    }
}
