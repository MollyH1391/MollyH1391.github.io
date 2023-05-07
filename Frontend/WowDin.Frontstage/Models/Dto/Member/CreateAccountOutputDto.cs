namespace WowDin.Frontstage.Models.Dto.Member
{
    public class CreateAccountOutputDto
    {
        public CreateAccountOutputDto()
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
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }

}
