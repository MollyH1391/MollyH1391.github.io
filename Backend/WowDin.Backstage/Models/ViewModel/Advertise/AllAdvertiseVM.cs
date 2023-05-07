using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Advertise
{
    public class AllAdvertiseVM
    {
        public IEnumerable<AdvertiseVM> AllAdvertise { get; set; }
    }

    public class AdvertiseVM
    {
        public int AdvertiseId { get; set; }
        public string Img { get; set; }
        public bool IsSearchZone { get; set; }
        public string CouponCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
