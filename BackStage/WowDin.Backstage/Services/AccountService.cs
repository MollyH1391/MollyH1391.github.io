using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WowDin.Backstage.Models.Dto.Account;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using System;

namespace WowDin.Backstage.Services
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
        public CreateAccountOutputDto CreateAccount(CreateAccountInputDto input)
        {
            var result = new CreateAccountOutputDto();
            result.IsSuccess = false;
            
            if (this.IsExistAccount(input.Email))
            {
                result.Message = "Email已經存在";
                return result;
            }

            if (this.IsExistBrandName(input.BrandName))
            {
                result.Message = "此品牌已註冊!";
                return result;
            }

            if (input.Password != input.PasswordCheck)
            {
                result.Message = "密碼、確認密碼不一致!";
                return result;
            }

            var verificationEntity = new Verification
            {
                Email = input.Email,
                Password = Encryption.SHA256Encrypt(input.Password),
                AccountType = 2
            };

            using (var tran = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    _repository.Create(verificationEntity);
                    _repository.Save();

                    var brandEntity = new Brand
                    {
                        Name = input.BrandName,
                        VerificationId = verificationEntity.VerificationId,
                        PayLevel = 1,
                        Logo= "https://i.imgur.com/BkxYFwk.png",
                        CardImgUrl = "https://i.imgur.com/U1uKQUi.jpeg",
                        Verified = 0,
                        Star = 0,
                        StarAmount = 0
                    };
                    _repository.Create(brandEntity);
                    _repository.Save();
                    var createBrand = new CreateBrandDto
                    {
                        Email= verificationEntity.Email,
                        Password=verificationEntity.Password,
                        AccountType=verificationEntity.AccountType,
                        BrandId=brandEntity.BrandId,
                        Name=brandEntity.Name,
                        VerificationId=brandEntity.VerificationId,
                        PayLevel=brandEntity.PayLevel,
                        Logo=brandEntity.Logo,
                        CardImgUrl=brandEntity.CardImgUrl,
                        Verified=brandEntity.Verified,
                        Star=brandEntity.Star,
                        StarAmount=brandEntity.StarAmount                
                    };
                    var brandHistory = new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = DateTime.UtcNow, UpdateTitle = "CreateBrand", UpdateContent = createBrand.JsonSerialize() };
                    _repository.Create(brandHistory);
                    _repository.Create(new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = brandHistory.UpdateDate, UpdateTitle = "BrandFacade", UpdateContent = "" });
                    _repository.Create(new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = brandHistory.UpdateDate, UpdateTitle = "BrandImages", UpdateContent = "" });
                    _repository.Create(new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = brandHistory.UpdateDate, UpdateTitle = "BrandIntroduce", UpdateContent = "" });
                    _repository.Create(new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = brandHistory.UpdateDate, UpdateTitle = "BrandWeb", UpdateContent = "" });
                    _repository.Create(new BrandHistory { BrandId = brandEntity.BrandId, UpdateDate = brandHistory.UpdateDate, UpdateTitle = "ShopImages", UpdateContent = "" });
                    _repository.Save();

                    result.IsSuccess = true;
                    result.BrandId = brandEntity.BrandId;

                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                }
            }


            return result;
        }

        public ForgetPasswordOutputDto ForgetPassword(ForgetPasswordInputDto input)
        {
            var result = new ForgetPasswordOutputDto();
            result.IsSuccess = false;
            result.Email = input.Email;

            if (!this.IsExistAccount(input.Email))
            {
                result.Message = "此帳號不存在!";
                return result;
            }

            var currentVerification = _repository.GetAll<Verification>().First(x => x.Email == input.Email);
            if (currentVerification.AccountType != 1 && currentVerification.AccountType != 2)
            {
                result.Message = "帳號身分錯誤!";
                return result;
            }

            //發重設密碼信
            _mailService.SendResetPasswordMail(input.Email);
            result.IsSuccess = true;
            return result;
        }

        public bool IsExistAccount(string email)
        {
            return _repository.GetAll<Verification>().Any(x => x.Email == email);
        }

        public bool IsExistBrandName(string name)
        {
            return _repository.GetAll<Brand>().Any(x => x.Name == name);
        }

        public LoginAccountOutputDto LoginAccount(LoginAccountInputDto input)
        {
            var result = new LoginAccountOutputDto();
            result.IsSuccess = false;

            if(string.IsNullOrWhiteSpace(input.Account) || string.IsNullOrWhiteSpace(input.Password))
            {
                result.Message = "帳號密碼不得為空!";
                return result;
            }

            if (!this.IsExistAccount(input.Account))
            {
                result.Message = "帳號或密碼錯誤!";
                return result;
            }

            var currentVerification = _repository.GetAll<Verification>().First(x => x.Email == input.Account);
            if (currentVerification.AccountType != 1 && currentVerification.AccountType != 2)
            {
                result.Message = "登入身分錯誤!";
                return result;
            }

            var currentBrand = _repository.GetAll<Brand>().First(x => x.VerificationId == currentVerification.VerificationId);
            if (currentBrand.Verified == 0)
            {
                result.Message = "品牌尚未通過審核，請耐心等候，將馬上為您處理!";
                return result;
            }
            if (currentBrand.Verified == 2)
            {
                result.Message = $"註冊品牌申請被拒! {currentBrand.Message}";
                return result;
            }
            if (currentBrand.Suspension)
            {
                result.Message = $"品牌已被停權! {currentBrand.Message}";
                return result;
            }
            if (Encryption.SHA256Encrypt(input.Password) != currentVerification.Password)
            {
                result.Message = "帳號或密碼錯誤!";
                return result;
            }

            result.IsSuccess = true;
            result.BrandId = currentBrand.BrandId;
            result.BrandName = currentBrand.Name;
            result.AccountType = currentVerification.AccountType;

            if (result.IsSuccess)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.BrandId.ToString()),
                    new Claim("UserRole", result.AccountType.ToString()),
                    new Claim("BrandName", result.BrandName.ToString())
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

        public ResetPasswordOutputDto ResetPassword(ResetPasswordInputDto input)
        {
            var result = new ResetPasswordOutputDto();
            result.IsSuccess = false;

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

        //public void VerifyAccount(int brandId)
        //{
        //    var brand = _repository.GetAll<Brand>().First(x => x.BrandId == brandId);
        //    if (brand.Verified)
        //    {
        //        brand.Verified = true;
        //        _repository.Update(brand);
        //        _repository.Save();
        //    }
        //}
    }
}
