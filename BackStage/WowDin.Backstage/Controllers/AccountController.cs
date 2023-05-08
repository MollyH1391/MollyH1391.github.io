using Microsoft.AspNetCore.Mvc;
using WowDin.Backstage.Models.Dto.Account;
using WowDin.Backstage.Models.ViewModel.Account;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public IActionResult ResetPassword(string email)
        {
            ViewData["email"] = email;
            return View();
        }

        public IActionResult Logout()
        {
            _accountService.LogoutAccount();
            return Redirect("/");
        }

        //--------Post

        [HttpPost]
        public IActionResult Login(LoginDataModel request)
        {
            var inputDto = new LoginAccountInputDto()
            {
                Account = request.Account,
                Password = request.Password
            };

            var outputDto = _accountService.LoginAccount(inputDto);
            if (outputDto.IsSuccess)
            {
                return Redirect("/");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View(request);
            }
        }

        [HttpPost]
        public IActionResult Signup(SignupDataModel request)
        {
            var inputDto = new CreateAccountInputDto()
            {
                BrandName = request.BrandName,
                Email = request.Email,
                Password = request.Password,
                PasswordCheck = request.PasswordCheck
            };

            var outputDto = _accountService.CreateAccount(inputDto);
            if (outputDto.IsSuccess)
            {
                TempData["Message"] = "註冊申請已送出，我們將盡快為您審核，請耐心等候。";
                return RedirectToAction("MessagePage", "MessageHandler");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View(request);
            }
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordDataModel request)
        {
            var inputDto = new ForgetPasswordInputDto()
            {
                Email = request.Email
            };

            var outputDto = _accountService.ForgetPassword(inputDto);

            if (outputDto.IsSuccess)
            {
                TempData["Message"] = "已寄送重設密碼連結至您的信箱!";
                return RedirectToAction("MessagePage", "MessageHandler");
            }
            else
            {
                ViewData["ErrorMessage"] = outputDto.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDataModel request)
        {
            var inputDto = new ResetPasswordInputDto()
            {
                Email = request.Email,
                NewPassword = request.Password,
                CheckNewPassword = request.PasswordCheck
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
                return View();
            }
        }
    }
}
