namespace WowDin.Backstage.Models.Dto.Advertise
{
    public class AdvertiseRequestDto
    {
        public int BrandId { get; set; }
        public string AdFig { get; set; }
        public bool IsSearchZone { get; set; }
        public int CouponId { get; set; }
    }
}
