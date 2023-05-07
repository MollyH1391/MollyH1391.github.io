using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Advertise
{
    public class AllAdvertiseDto
    {
        public List<AdvertiseDto> AllAdvertise { get; set; }
    }

    public class AdvertiseDto
    {
        public int AdvertiseId { get; set; }
        public string Img { get; set; }
        public bool IsSearchZone { get; set; }
        public string CouponCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
