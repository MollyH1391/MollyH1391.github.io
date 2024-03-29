﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using WowDin.Frontstage.Services.Interface;

namespace WowDin.Frontstage.Services
{
    public class MailService : IMailService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public MailService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        private SmtpClient InitClient()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential(_config["MailCredential:UserName"], _config["MailCredential:Password"]);
            client.EnableSsl = true;
            return client;
        }
        private MailMessage CreateNormalMail(string mailTo, string mailSubject, string mailBody)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_config["MailCredential:UserName"], _config["MailCredential:DisplayName"]);
            mail.To.Add(mailTo);
            mail.Priority = MailPriority.Normal;
            mail.Subject = mailSubject;
            mail.IsBodyHtml = true;
            mail.Body = mailBody;
            return mail;
        }

        public void SendResetPasswordMail(string mailTo)
        {
            SmtpClient client = InitClient();

            var mailSubject = "Wowdin密碼重設信";
            var mailBody = $@"
                <h3>請點擊以下連結重設密碼:</h3>
                <a href='https://{_httpContextAccessor.HttpContext.Request.Host.Value}/Member/ResetPassword?email={mailTo}' target='_blank'>連結</a>
                <p>若您非Wowdin會員，請忽略此信件。<br/>若您為Wowdin會員，但並未進行此操作，請立即修改您的密碼，以保障您的帳號安全。</p>
            ";
            MailMessage mail = CreateNormalMail(mailTo, mailSubject, mailBody);
            client.Send(mail);
        }

        public void SendVerifyMail(string mailTo, int userAccountId)
        {
            SmtpClient client = InitClient();

            //mail
            var mailSubject = "Wowdin帳號驗證信";
            var mailBody = @$"
                <h3>點擊以下連結以啟用帳戶:</h3>
                <a href='https://{_httpContextAccessor.HttpContext.Request.Host.Value}/Member/Verify?user={userAccountId}' target='_blank'>連結</a>
            ";
            MailMessage mail = CreateNormalMail(mailTo, mailSubject, mailBody);

            client.Send(mail);
        }
    }
}
