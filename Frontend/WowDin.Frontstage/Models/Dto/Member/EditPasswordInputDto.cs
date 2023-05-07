namespace WowDin.Frontstage.Models.Dto.Member
{
    public class EditPasswordInputDto
    {
        public int UserAccountId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string CheckNewPassword { get; set; }
    }
}
