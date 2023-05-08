namespace WowDin.Frontstage.Services.Interface
{
    public interface IMailService
    {
        public void SendVerifyMail(string mailTo, int userAccountId);
        public void SendResetPasswordMail(string mailTo);
    }
}
