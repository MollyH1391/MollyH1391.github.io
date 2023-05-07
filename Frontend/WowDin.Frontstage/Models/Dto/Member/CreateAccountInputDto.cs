namespace WowDin.Frontstage.Models.Dto.Member
{
    public class CreateAccountInputDto
    {
        public string Email { get; set; }
        public string Realname { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordCheck { get; set; }
    }
}
