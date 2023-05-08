using System.ComponentModel.DataAnnotations;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class SignupDataModel
    {
        //驗證寫在這[]
        public string Email { get; set; }
        public string Realname { get; set; }
        public string Nickname { get; set; }

        [RegularExpression(@"^09\d{8}$", ErrorMessage = "需為09開頭共10碼之純數字格式")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請輸入密碼!")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "最少需6個位元")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,64}$", ErrorMessage = "密碼須為英數字混合!")]
        public string Password { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,64}$", ErrorMessage = "密碼須為英數字混合!")]
        public string PasswordCheck { get; set; }
    }
}
