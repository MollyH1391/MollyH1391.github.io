namespace WowDin.Backstage.Models.Dto.Account
{
    public class LoginAccountOutputDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int AccountType { get; set; }
        
    }
}
