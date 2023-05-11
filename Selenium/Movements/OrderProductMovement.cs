using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTestProject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestProject.Movements
{
    internal class OrderProductMovement
    {
        private IWebDriver _driver;
        private WebDriverWait wait;
        IJavaScriptExecutor jse;

        public OrderProductMovement(IWebDriver driver)
        {
            _driver = driver;
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1000));
            jse = (IJavaScriptExecutor)_driver;
        }
        
        public void OrderSomethingRandomOnCurrentMenu()
        {
            ClickProduct();
            SetCustom();
            AddToCart();
        }


        public void OrderSomethingHasDiscountOnSpecificMenu(int shopId, string couponProduct)
        {
            _driver.Navigate().GoToUrl($"https://wowdin.azurewebsites.net/Menu/{shopId}");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);

            ClickProduct(couponProduct);
            SetCustom();
            AddAmount(1);
            AddToCart();
        }

        public void GotoCartByClick()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("menu_cart")));

            var cartAnchor = _driver.FindElement(By.ClassName("menu_cart"));
            jse.ExecuteScript("arguments[0].click();", cartAnchor);
            //cartAnchor.Click();
        }

        public string GetGroupURL()
        {
            var groupBtn = _driver.FindElement(By.Id("menu_group_btn"));
            jse.MoveAndClickByJS(groupBtn);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementValue(By.Id("menu_web_address"), "https://"));
            var groupUrl = _driver.FindElement(By.Id("menu_web_address")).GetAttribute("value");
            
            return groupUrl;
        }

        private void AddToCart()
        {
            var addProductBtn = _driver.FindElement(By.Id("go_to_cart"));
            addProductBtn.Click();
        }

        private void AddAmount(int times)
        {
            var addAmountBtn = _driver.FindElement(By.Id("add-quentity"));
            for(int i = 0; i < times; i++)
            {
                addAmountBtn.Click();
            }
        }

        private void ReduceAmount(int times)
        {
            var reduceAmountBtn = _driver.FindElement(By.Id("reduce-quentity"));
            for (int i = 0; i <= times; i++)
            {
                reduceAmountBtn.Click();
            }
        }


        private void SetNickname(string nickname)
        {
            var nameInput = _driver.FindElement(By.Id("menu_item_modal_input"));
            nameInput.SendKeys(nickname);
        }

        private void ClickProduct(string productName)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".menu_main_col")));

            var products = _driver.FindElements(By.CssSelector(".menu_borad_item > button"));
            foreach (var product in products)
            {
                if (product.FindElement(By.ClassName("menu_borad_item_name")).Text.Contains(productName))
                {
                    jse.MoveAndClickByJS(product);
                }
            }
        }
        private void ClickProduct()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".menu_borad_item")));

            var products = _driver.FindElements(By.CssSelector(".menu_borad_item > button"));
            var rdm = new Random();
            Console.WriteLine(products.Count);
            var product = products[rdm.Next(0, products.Count)];

            jse.MoveAndClickByJS(product);
        }

        private void SetCustom()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".custom-body label.btn")));
            SetNickname("hihi");
            var customs = _driver.FindElements(By.CssSelector(".custom-body .group-body"));
            foreach (var custom in customs)
            {
                custom.FindElement(By.CssSelector("label.btn")).Click();
            }
        }
    }
}
