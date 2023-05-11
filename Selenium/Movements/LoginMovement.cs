using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestProject.Movements
{
    internal class LoginMovement
    {
        private IWebDriver _driver;
        public LoginMovement(IWebDriver driver)
        {
            _driver = driver;
        }

        public void LoginByEmail(string email, string password)
        {
            _driver.Navigate().GoToUrl("https://wowdin.azurewebsites.net/Member/Login");
            var accountInput = _driver.FindElement(By.Name("Account"));
            var passwordInput = _driver.FindElement(By.Name("Password"));
            var loginBtn = _driver.FindElement(By.CssSelector(".submit-btn"));
            
            accountInput.SendKeys(email);
            passwordInput.SendKeys(password);
            loginBtn.Click();
        }
    }
}
