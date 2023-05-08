using WowDin.Frontstage.Models.Dto.Member;
using WowDin.Frontstage.Models.ViewModel.Member;

namespace WowDin.Frontstage.Services.Interface
{
    public interface IAccountService
    {
        public CreateAccountOutputDto CreateAccont(CreateAccountInputDto input);
        public LoginAccountOutputDto LoginAccount(LoginAccountInputDto input);
        public ForgetPasswordOutputDto ForgetPassword(ForgetPasswordInputDto input);
        public ResetPasswordOutputDto ResetPassword(ResetPasswordInputDto input);
        public EditPasswordOutputDto EditPassword(EditPasswordInputDto input);
        public void LogoutAccount();
        public bool IsExistAccount(string email, int loginType);
        public void VerifyAccount(int userAccountId);
        public void SocialLogin(SocialLoginInputDto input);
        public void SocialCreateAccount(SocialLoginInputDto input);

    }
}
