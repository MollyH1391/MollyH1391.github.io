using System.ComponentModel.DataAnnotations;

namespace WowDin.Frontstage.Models.Dto.Member
{
    public class LoginAccountInputDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
