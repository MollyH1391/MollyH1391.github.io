namespace WowDin.Backstage.Services.Interface
{
    public interface IMailService
    {
        void SendResetPasswordMail(string mailTo);
    }
}
