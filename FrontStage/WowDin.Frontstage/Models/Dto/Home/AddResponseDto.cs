using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowDin.Frontstage.Models.Dto.Home
{
    public class AddResponseDto
    {
        public int UserAccountId { get; set; }
        public int? BrandId { get; set; }
        public int? ShopId { get; set; }
        public string ResponseContent { get; set; }
    }
}
