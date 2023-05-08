using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowDin.Frontstage.Models.Dto.Home
{
    public class ResponseDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Brands> Brand { get; set; }
        //public IEnumerable<string> Shop { get; set; }
        public string ResponseContent { get; set; }
    }
    public class Brands
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public List<Shops> Shop { get; set; }
    }
    public class Shops
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }
}
