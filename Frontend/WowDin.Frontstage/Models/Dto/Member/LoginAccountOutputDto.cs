namespace WowDin.Frontstage.Models.Dto.Member
{
    public class LoginAccountOutputDto
    {
        public LoginAccountOutputDto()
        {
            User = new UserData();
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserData User { get; set; }
        public class UserData
        {
            public int UserAccountId { get; set; }
            public string Realname { get; set; }
            public string Nickname { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int AccountType { get; set; }
        }
    }
}
