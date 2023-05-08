using System.ComponentModel.DataAnnotations;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class MemberEditPasswordDataModel
    {
        public int UserAccountId { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,64}$", ErrorMessage = "密碼須為英數字混合!")]
        public string Password { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,64}$", ErrorMessage = "密碼須為英數字混合!")]
        public string NewPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,64}$", ErrorMessage = "密碼須為英數字混合!")]
        public string CheckNewPassword { get; set; }

    }
}
