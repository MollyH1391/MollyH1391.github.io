namespace WowDin.Backstage.Models.Dto.Account
{
    public class CreateBrandDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccountType { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int VerificationId { get; set; }
        public int PayLevel { get; set; }
        public string CardImgUrl { get; set; }
        public int Star { get; set; }
        public int StarAmount { get; set; }
        public int Verified { get; set; }
    }
}
