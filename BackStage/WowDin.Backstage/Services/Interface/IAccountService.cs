using WowDin.Backstage.Models.Dto.Account;

namespace WowDin.Backstage.Services.Interface
{
    public interface IAccountService
    {
        CreateAccountOutputDto CreateAccount(CreateAccountInputDto input);
        LoginAccountOutputDto LoginAccount(LoginAccountInputDto input);
        ForgetPasswordOutputDto ForgetPassword(ForgetPasswordInputDto input);
        ResetPasswordOutputDto ResetPassword(ResetPasswordInputDto input);
        void LogoutAccount();
        bool IsExistAccount(string email);
        bool IsExistBrandName(string name);
        //void VerifyAccount(int brandId);
    }
}
