using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WowDin.Frontstage.Models;
using WowDin.Frontstage.Models.ViewModel.Member;
using WowDin.Frontstage.Services.Member.Interface;
using System.Collections.Generic;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Models.Dto;
using WowDin.Frontstage.Common.ModelEnum;
using System;
using WowDin.Frontstage.Services.Member;
using WowDin.Frontstage.Services;
using WowDin.Frontstage.Models.ViewModel.Home;
using WowDin.Frontstage.Models.Dto.Member;
using System.Threading.Tasks;
using WowDin.Frontstage.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WowDin.Frontstage.Models.Dto.Home;
using AutoMapper;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WowDin.Frontstage.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public MemberController(IMemberService memberService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _memberService = memberService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }
        private string RedirectUrl => "https://" + HttpContext.Request.Host.ToString();

        [TempData]
        public Guid State { get; set; }

        [Authorize]
        public IActionResult ResponsePage()
        {
            var id = int.Parse(User.Identity.Name);
            //id = 1;
            var person = _memberService.InitialResponse(id);

            var result = new ResponseViewModel()
            {
                Brand = person.Brand,
                Name = person.Name,
                Phone = person.Phone
            };

            return View(result);
        }
        [HttpPost]
        public void AddResponseData(AddResponseDto request)
        {
            request.UserAccountId = int.Parse(User.Identity.Name);
            _memberService.AddReponse(request);
        }

        public JsonResult GetBrandData()
        {
            var id = int.Parse(User.Identity.Name);

            var person = _memberService.InitialResponse(id);

            return Json(person.Brand);
        }


        //---------------------
        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Login()
        {
            State = Guid.NewGuid();
            //Line
            ViewData["LineAuth"] = $"https://access.line.me/oauth2/v2.1/authorize?" +
                $"client_id={_config["Line:ClientId"]}" +
                $"&response_type=code" +
                $"&redirect_uri={RedirectUrl}/Member/LineLogin" +
                $"&scope={HttpUtility.UrlEncode("profile openid email")}" +
                $"&state={State}";
            //Google
            ViewData["GoogleAuth"] = $"https://accounts.google.com/o/oauth2/auth?" +
                $"scope={HttpUtility.UrlEncode("profile email")}" +
                $"&response_type=code" +
                $"&state={State}" +
                $"&redirect_uri={RedirectUrl}/Member/GoogleLogin" +
                $"&client_id={_config["Google:ClientId"]}";

            return View();
        }


        public async Task<IActionResult> LineLogin(string code, Guid state, string error, string error_description)
        {
            //有錯誤訊息(未授權等)、state遺失、state不相同、沒有code
            if (!string.IsNullOrEmpty(error) || state == null || State != state || string.IsNullOrEmpty(code))
            {
                ViewData["ErrorMessage"] = error_description;
                return RedirectToAction("Login");
            }

            var url = "https://api.line.me/oauth2/v2.1/token";
            var postData = new Dictionary<string, string>()
            {
                {"client_id", _config["Line:ClientId"]},
                {"client_secret", _config["Line:ClientSecret"]},
                {"code", code },
                {"grant_type", "authorization_code" },
                {"redirect_uri", $"{RedirectUrl}/Member/LineLogin" }
            };

            var contentPost = new FormUrlEncodedContent(postData);
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(url, contentPost);

            string responseContent;
            if (response.IsSuccessStatusCode)
                responseContent = await response.Content.ReadAsStringAsync();
            else
                //ViewData["ErrorMessage"] = error_description;
                return RedirectToAction("Login");

            var lineLoginResource = JsonConvert.DeserializeObject<LINELoginResource>(responseContent);

            //取得使用者資訊，執行登入或註冊
            var userInfo = new JwtSecurityToken(lineLoginResource.IDToken).Payload;
            var email = userInfo.First(x => x.Key == "email").Value.ToString();
            var name = userInfo.First(x => x.Key == "name").Value.ToString();
            var photo = userInfo.First(x => x.Key == "picture").Value.ToString();

            var inputDto = new SocialLoginInputDto()
            {
                Account = email,
                Name = name,
                Photo = photo,
                LoginType = 3
            };

            if (!_accountService.IsExistAccount(email, 3))
            {
                //先註冊
                _accountService.SocialCreateAccount(inputDto);
            }
            //再登入
            _accountService.SocialLogin(inputDto);

            return Redirect("/");
        }

        public IActionResult Verify(int user)
        {
            _accountService.VerifyAccount(user);
            TempData["Message"] = "信箱驗證成功，請重新登入。";
            return RedirectToAction("MessagePage", "MessageHandler");
        }
        public IActionResult Logout()
        {
            _accountService.LogoutAccount();
            return Redirect("/");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        //-------Post

        [HttpPost]
        public IActionResult FacebookLogin([FromBody] FbLoginDataModel request)
        {
            var inputDto = new SocialLoginInputDto()
            {
                Account = request.Email,
                Name = request.Name,
                LoginType = 1
            };

            if (!_accountService.IsExistAccount(request.Email, 1))
            {
                //先註冊
                _accountService.SocialCreateAccount(inputDto);
            }
            //再登入
            _accountService.SocialLogin(inputDto);
            //if (Url.IsLocalUrl(request.ReturnUrl)) return Redirect(request.ReturnUrl);
            return Redirect("/");

        }
        
        [HttpPost]
        public IActionResult Signup(SignupDataModel request)
        {
            var inputDto = new CreateAccountInputDto
            {
                Email = request.Email,
                Realname = request.Realname,
                Nickname = request.Nickname,
                Phone = request.Phone,
                Password = request.Password,
                PasswordCheck = request.PasswordCheck
            };

            var outputDto = _accountService.CreateAccont(inputDto);

            if (outputDto.IsSuccess)
            {
                TempData["Message"] = "驗證信已寄出，請至您的信箱完成註冊流程。";
                return RedirectToAction("MessagePage", "MessageHandler");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View("Signup", request);
            }
        }

        [HttpPost]
        public IActionResult Login(LoginDataModel request, string returnUrl)
        {
            var inputDto = new LoginAccountInputDto()
            {
                Account = request.Account,
                Password = request.Password
            };
            //登入
            var outputDto = _accountService.LoginAccount(inputDto);

            if (outputDto.IsSuccess)
            {
                if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return Redirect("/");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View("Login", request);
            }

        }

        [HttpPost]
        public IActionResult ForgetPassword(MemberForgetPasswordDataModel request)
        {
            var forgetPasswordDto = new ForgetPasswordInputDto()
            {
                Email = request.Email
            };
            var outputDto = _accountService.ForgetPassword(forgetPasswordDto);

            if (outputDto.IsSuccess)
            {
                TempData["Message"] = "已寄送重設密碼連結至您的信箱!";
                return RedirectToAction("MessagePage", "MessageHandler");

            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View("ForgetPassword", request);
            }

            //return View(nameof(ForgetPassword));
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            
            ViewData["email"] = email;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(MemberResetPasswordDataModel requset)
        {
            var inputDto = new ResetPasswordInputDto()
            {
                Email = requset.Email,
                NewPassword = requset.NewPassword,
                CheckNewPassword = requset.CheckNewPassword
            };
            var outputDto = _accountService.ResetPassword(inputDto);
            if (outputDto.IsSuccess)
            {
                TempData["Message"] = "重設密碼成功!請重新登入";
                return RedirectToAction("MessagePage", "MessageHandler");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View("ResetPassword");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditPassword()
        {
            
            ViewData["id"] = User.Identity.Name;
            //User.Claims.First(x => x.Type == "Email");
            
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditPassword(MemberEditPasswordDataModel request)
        {
            var inputDto = new EditPasswordInputDto()
            {
                UserAccountId = int.Parse(User.Identity.Name),
                Password = request.Password,
                NewPassword = request.NewPassword,
                CheckNewPassword = request.CheckNewPassword
            };

            var result = _accountService.EditPassword(inputDto);
            if (result.IsSuccess)
            {
                _accountService.LogoutAccount();
                TempData["Message"] = "密碼修改成功!請重新登入";
                return RedirectToAction("MessagePage", "MessageHandler");
            }
            else
            {
                ViewData["ErrorMessage"] = result.Message;
                return View("EditPassword", request);
            }

        }

        

        [Authorize]
        public IActionResult MemberCardList(int id)
        {
            id = int.Parse(User.Identity.Name);
            var memberCardList = _memberService.GetCardListById(id);
            var memberCardListViewModel = memberCardList.Select(x => new MemberCardListViewModel()
            {
                BrandId = x.BrandId,
                CardImgUrl = x.CardImgUrl
            });
            return View(memberCardListViewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Account(int id)
        {
            id = int.Parse(User.Identity.Name);
            var user = _memberService.GetUserDataById(id);

            MemberAccountViewModel accountViewModel = new MemberAccountViewModel()
            {
                UserAccountId = id,
                LoginType = user.LoginType,
                RealName = user.RealName,
                NickName = user.NickName,
                Phone = user.Phone,
                Sex = user.Sex,
                Birthday = user.Birthday.ToString("yyyy-MM-dd"),
                Photo = user.Photo,
                City = user.City,
                Distrinct = user.Distrinct,
                Point = user.Point,
                CouponAmount = user.CouponAmount
            };

            return View(accountViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Account(UserAccountDataModel user)
        {
            UpdateUserDataDto updateUserDataDto = new UpdateUserDataDto()
            {
                UserAccountId = user.UserAccountId,
                RealName = user.RealName.Trim(),
                NickName = user.NickName.Trim(),
                Photo = user.Photo,
                Sex = user.Sex,
                Birthday = user.Birthday,
                Phone = user.Phone?.Trim(),
                City = user.City,
                Distrinct = user.Distrinct
            };
            var isSuccess = _memberService.UpdateUserData(updateUserDataDto);

            if (isSuccess) return RedirectToAction("Account", user.UserAccountId);
            else return View(user); 
        }

        [Authorize]
        public IActionResult MemberCard(MemberCardDataModel request)
        {
            var userId = int.Parse(User.Identity.Name);
            
            var user = _memberService.GetUserDataById(userId);
            var cardData = _memberService.GetCardListById(userId).First(x => x.BrandId == request.BrandId);
            MemberMembercardViewModel memberCardData = new MemberMembercardViewModel()
            {
                LoginType = user.LoginType,
                RealName = user.RealName,
                NickName = user.NickName,
                Phone = user.Phone,
                Sex = user.Sex,
                Birthday = user.Birthday.ToString("yyyy-MM-dd"),
                Photo = user.Photo,
                City = user.City,
                Distrinct = user.Distrinct,
                Point = cardData.Point,
                CardImgUrl = cardData.CardImgUrl,
                CouponAmount = user.CouponAmount,
                CardTypeName = cardData.CardTypeName,
                NextCardTypeName = cardData.NextCardTypeName,
                Range = cardData.Range
            };

            return View(memberCardData);

        }
    }
}
