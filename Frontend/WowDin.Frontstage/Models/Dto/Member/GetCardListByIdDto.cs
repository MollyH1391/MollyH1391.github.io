namespace WowDin.Frontstage.Models.Dto.Member
{
    public class GetCardListByIdDto
    {
        public int UserAccountId { get; set; }
        public int BrandId { get; set; }
        public string CardImgUrl { get; set; }
        public string CardTypeName { get; set; }
        public string NextCardTypeName { get; set; }
        public int Range { get; set; }
        public int Point { get; set; }
    }
}
