namespace WowDin.Frontstage.Models.Dto.Member
{
    public class ResetPasswordInputDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string CheckNewPassword { get; set; }
    }
}
