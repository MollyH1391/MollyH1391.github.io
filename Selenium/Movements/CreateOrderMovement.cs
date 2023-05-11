using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTestProject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumTestProject.Movements
{
    internal class CreateOrderMovement
    {
        private IWebDriver _driver;
        private WebDriverWait wait;
        IJavaScriptExecutor jse;

        public CreateOrderMovement(IWebDriver driver)
        {
            _driver = driver;
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1000));
            jse = (IJavaScriptExecutor)_driver;
            
        }

        public string SetCartAndCreateOrder(int shopId, int? couponId)
        {
            if (!_driver.Url.Contains("https://wowdin.azurewebsites.net/Order/CartDetail/"))
            {
                _driver.Navigate().GoToUrl($"https://wowdin.azurewebsites.net/Order/CartDetail/{shopId}");
            }
            var priceToPay = StepOne(couponId);

            var nextBtn = _driver.FindElement(By.Id("partialControlBtn"));
            jse.MoveAndClickByJS(nextBtn);

            StepTwo(true);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("CartS3")));

            return priceToPay;
        }

        private string StepOne(int? couponId)
        {
            var takeLabel = _driver.FindElement(By.CssSelector("[value='自取'] + label"));
            jse.MoveAndClickByJS(takeLabel);

            var pickUpDate = _driver.FindElement(By.Id("showPickUpDate"));
            jse.MoveAndClickByJS(pickUpDate);
            
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#showCalendar button.bg_blue")));
            
            var selectableDates = _driver.FindElements(By.CssSelector("#showCalendar button.bg_blue"));
            selectableDates[1].Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("#showCalendar")));

            var pickUpTime = _driver.FindElement(By.Id("showPickUpTime"));
            jse.MoveAndClickByJS(pickUpTime);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("checkTimePickUp")));
            
            var pickUpTimeCheckBtn = _driver.FindElement(By.Id("checkTimePickUp"));
            jse.MoveAndClickByJS(pickUpTimeCheckBtn);
            
            Thread.Sleep(1000);

            //SelectCoupon("阿薩姆紅茶");
            SelectCoupon(couponId);

            var finalPrice = _driver.FindElement(By.Id("summary_final_price")).Text;

            return finalPrice.Trim();
        }

        private void SelectCoupon(string couponTitle)
        {
            var couponSelector = _driver.FindElement(By.Id("coupon_selector"));
            jse.MoveAndClickByJS(couponSelector);

            var options = _driver.FindElements(By.CssSelector("#coupon_selector option"));
            foreach(var option in options)
            {
                if (option.Text.Contains(couponTitle))
                {
                    option.Click();
                }
            }
        }

        private void SelectCoupon(int? couponId)
        {
            if(couponId == 0 || couponId == null)
            {
                return;
            }
            
            var couponSelector = _driver.FindElement(By.Id("coupon_selector"));
            jse.MoveAndClickByJS(couponSelector);

            var options = _driver.FindElements(By.CssSelector("#coupon_selector option"));
            foreach (var option in options)
            {
                if (option.GetAttribute("value") == couponId.ToString())
                {
                    option.Click();
                }
            }
        }

        private void StepTwo(bool byCreditCard)
        {
            var phone = _driver.FindElement(By.Id("phone"));
            if (string.IsNullOrEmpty(phone.Text))
            {
                jse.MoveAndClickByJS(phone);
                phone.SendKeys("0987654321");
            }

            if (byCreditCard)
            {
                var creditCardLabel = _driver.FindElement(By.CssSelector("[value='刷卡'] + label"));
                jse.MoveAndClickByJS(creditCardLabel);
            }

            var invoiceLabel = _driver.FindElement(By.CssSelector("[value='紙本發票']"));
            jse.MoveAndClickByJS(invoiceLabel);

            var nextBtn = _driver.FindElement(By.Id("checkDetail"));
            jse.MoveAndClickByJS(nextBtn);
        }

        public void SendOrder()
        {
            var sendOutBtn = _driver.FindElement(By.Id("makeOrderBtn"));
            jse.MoveAndClickByJS(sendOutBtn);
        }

        public void CreditCard(string phone)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches("https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5"));

            _driver.FindElement(By.Id("CCpart1")).SendKeys("4311");
            _driver.FindElement(By.Id("CCpart2")).SendKeys("9522");
            _driver.FindElement(By.Id("CCpart3")).SendKeys("2222");
            _driver.FindElement(By.Id("CCpart4")).SendKeys("2222");

            _driver.FindElement(By.Id("creditMM")).SendKeys("12");
            _driver.FindElement(By.Id("creditYY")).SendKeys("26");

            _driver.FindElement(By.Id("CreditBackThree")).SendKeys("222");

            _driver.FindElement(By.Id("CellPhoneCheck")).SendKeys(phone);
        }

    }
}
