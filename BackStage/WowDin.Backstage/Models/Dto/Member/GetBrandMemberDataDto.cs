namespace WowDin.Backstage.Models.Dto.Member
{
    public class GetBrandMemberDataDto
    {
        public int UserAccountId { get; set; }
        public int BrandId { get; set; }
        public string NickName { get; set; }
        public int Point { get; set; }
        public string GradeName { get; set; }
    }
}
