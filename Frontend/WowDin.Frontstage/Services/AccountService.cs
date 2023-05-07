using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Models.Dto.Member;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Models.ViewModel.Member;
using WowDin.Frontstage.Repositories.Interface;
using WowDin.Frontstage.Services.Interface;

namespace WowDin.Frontstage.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(IRepository repository, IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SocialLogin(SocialLoginInputDto input)
        {
            string password = "";
            switch (input.LoginType)
            {
                case 1:
                    password = "FB登入";
                    break;
                case 2:
                    password = "Google登入";
                    break;
                case 3:
                    password = "Line登入";
                    break;
            }

            var currentVerification = _repository.GetAll<Verification>().First(x => x.Email == input.Account && x.Password == password);
            var currentUser = _repository.GetAll<UserAccount>().First(x => x.VerificationId == currentVerification.VerificationId);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, currentUser.UserAccountId.ToString()),
                new Claim(ClaimTypes.Email, currentVerification.Email),
                new Claim("RealName", currentUser.RealName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            
        }
        public void SocialCreateAccount(SocialLoginInputDto input)
        {
            string password = "";
            switch (input.LoginType)
            {
                case 1:
                    password = "FB登入";
                    break;
                case 2:
                    password = "Google登入";
                    break;
                case 3:
                    password = "Line登入";
                    break;
            }

            //mapping
            var verificationEntity = new Verification
            {
                Email = input.Account,
                Password = password,
                AccountType = 0
            };

            using (var tran = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    _repository.Create(verificationEntity);
                    _repository.Save();
                    var userEntity = new UserAccount
                    {
                        UserStamp = DateTimeOffset.Now.ToString(),
                        LoginType = input.LoginType,
                        RealName = input.Name,
                        NickName = input.Name,
                        Photo = input.Photo ?? "https://i.imgur.com/PytuV1J.jpeg",
                        Sex = 2,
                        Birthday = null,
                        Phone = null,
                        City = null,
                        Distrinct = null,
                        Verified = true,
                        VerificationId = verificationEntity.VerificationId
                    };
                    _repository.Create(userEntity);
                    _repository.Save();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            
        }
        public ForgetPasswordOutputDto ForgetPassword(ForgetPasswordInputDto input)
        {
            var result = new ForgetPasswordOutputDto();
            result.IsSuccess = false;
            result.Email = input.Email;

            //檢核
            if (!this.IsExistAccount(input.Email, 0))
            {
                result.Message = "使用者帳號不存在!";
                return result;
            }

            //發重設密碼信
            _mailService.SendResetPasswordMail(result.Email);
            result.IsSuccess = true;
            return result;
        }
        public ResetPasswordOutputDto ResetPassword(ResetPasswordInputDto input)
        {
            var result = new ResetPasswordOutputDto
            {
                IsSuccess = false
            };

            //檢核
            if (input.NewPassword != input.CheckNewPassword)
            {
                result.Message = "密碼、確認密碼不一致!";
                return result;
            }

            var currentVerification = _repository.GetAll<Verification>().First(x => x.Email == input.Email);
            currentVerification.Password = Encryption.SHA256Encrypt(input.NewPassword);
            _repository.Update(currentVerification);
            _repository.Save();
            result.IsSuccess = true;
            return result;
        }


        public EditPasswordOutputDto EditPassword(EditPasswordInputDto input)
        {
            var outputDto = new EditPasswordOutputDto()
            {
                IsSuccess = false
            };
            var currentUser = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == input.UserAccountId);

            var currentVerification = _repository.GetAll<Verification>().First(x => x.VerificationId == currentUser.VerificationId);

            //檢核
            if(currentVerification.Password != Encryption.SHA256Encrypt(input.Password))
            {
                outputDto.Message = "舊密碼輸入錯誤!";
                return outputDto;
            }

            if(currentVerification.Password == Encryption.SHA256Encrypt(input.NewPassword))
            {
                outputDto.Message = "新密碼不可與舊密碼相同!";
                return outputDto;
            }

            if(input.NewPassword != input.CheckNewPassword)
            {
                outputDto.Message = "密碼、確認密碼不一致!";
                return outputDto;
            }

            currentVerification.Password = Encryption.SHA256Encrypt(input.NewPassword);
            _repository.Update(currentVerification);
            _repository.Save();
            outputDto.IsSuccess = true;
            return outputDto;
        }
        public CreateAccountOutputDto CreateAccont(CreateAccountInputDto input)
        {
            var result = new CreateAccountOutputDto();
            result.IsSuccess = false;
            result.User.Nickname = input.Nickname;
            result.User.Email = input.Email;
            result.User.Phone = input.Phone;

            //檢核
            if (this.IsExistAccount(input.Email, 0))
            {
                result.Message = "Email已經存在";
                return result;
            }

            if(input.Password != input.PasswordCheck)
            {
                result.Message = "密碼、確認密碼不一致!";
                return result;
            }

            //mapping
            var verificationEntity = new Verification
            {
                Email = input.Email,
                Password = Encryption.SHA256Encrypt(input.Password),
                AccountType = 0
            };

            var userEntity = new UserAccount();
            using (var tran = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    _repository.Create(verificationEntity);
                    _repository.Save();
                    userEntity = new UserAccount
                    {
                        UserStamp = DateTimeOffset.Now.ToString(),
                        LoginType = 0,
                        RealName = input.Realname,
                        NickName = input.Nickname,
                        Photo = "https://i.imgur.com/PytuV1J.jpeg",
                        Sex = 2,
                        Birthday = null,
                        Phone = input.Phone,
                        City = null,
                        Distrinct = null,
                        Verified = false,
                        VerificationId = verificationEntity.VerificationId
                    };
                    _repository.Create(userEntity);
                    _repository.Save();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            //寄驗證信
            _mailService.SendVerifyMail(verificationEntity.Email, userEntity.UserAccountId);

            result.IsSuccess = true;
            result.User.UserAccountId = userEntity.UserAccountId;

            return result;
        }

        public bool IsExistAccount(string email, int loginType)
        {
            //找符合的email、accountType == 0，找對應的UserAccount、Any(logintype == loginType)
            var verificationList = _repository.GetAll<Verification>().Where(x => x.Email == email && x.AccountType == 0).ToList();
            var userAccountList = _repository.GetAll<UserAccount>().ToList();
            return userAccountList.Where(x => verificationList.Select(y => y.VerificationId).Contains(x.VerificationId)).Any(x => x.LoginType == loginType);
        }

        public LoginAccountOutputDto LoginAccount(LoginAccountInputDto input)
        {
            var result = new LoginAccountOutputDto();
            result.IsSuccess = false;

            //檢核
            if (!this.IsExistAccount(input.Account, 0))
            {
                result.Message = "帳號或密碼錯誤!";
                return result;
            }

            var currentVerification = _repository.GetAll<Verification>().First(x => x.Email == input.Account);
            var currentUser = _repository.GetAll<UserAccount>().First(x => x.VerificationId == currentVerification.VerificationId);

            if (!currentUser.Verified)
            {
                result.Message = "使用者帳號尚未驗證";
                return result;
            }

            if(Encryption.SHA256Encrypt(input.Password) != currentVerification.Password)
            {
                result.Message = "帳號或密碼錯誤!";
                return result;
            }

            result.IsSuccess = true;
            result.User.UserAccountId = currentUser.UserAccountId;
            result.User.Phone = currentUser.Phone;
            result.User.Email = currentVerification.Email;
            result.User.Realname = currentUser.RealName;
            result.User.Nickname = currentUser.NickName;
            result.User.AccountType = currentVerification.AccountType;

            if (result.IsSuccess)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.User.UserAccountId.ToString()),
                    new Claim(ClaimTypes.Email, result.User.Email),
                    //UserRole
                    new Claim("RealName", result.User.Realname),
                    new Claim("NickName", result.User.Nickname),
                    new Claim("Phone", result.User.Phone)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            }

            return result;
        }

        public void LogoutAccount()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public void VerifyAccount(int userAccountId)
        {
            var user = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == userAccountId);
            if (!user.Verified)
            {
                user.Verified = true;
                _repository.Update(user);
                _repository.Save();
            }
        }

        
    }
}
