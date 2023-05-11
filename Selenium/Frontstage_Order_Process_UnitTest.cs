using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumTestProject.Movements;
using System;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace SeleniumTestProject
{
    public class Tests
    {
        IWebDriver driver;
        IJavaScriptExecutor jse;
        WebDriverWait wait;
        OrderProductMovement orderProduct;
        CreateOrderMovement createOrder;
        LoginMovement login;


        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Set up");
            new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeOptions options = new ChromeOptions();

            driver = new ChromeDriver(); //Open browser
            driver.Manage().Window.Maximize();

            jse = (IJavaScriptExecutor)driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1000));
            orderProduct = new OrderProductMovement(driver);
            createOrder = new CreateOrderMovement(driver);
            login = new LoginMovement(driver);
        }

        // This C# Selenium method tests if the user must log in before placing an order
        [Test]
        public void ProcessWithoutLogin()
        {
            driver.Navigate().GoToUrl("https://wowdin.azurewebsites.net");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
            jse.ExecuteScript("scroll(0, 1000)");

            var findMoreBtn = driver.FindElement(By.CssSelector(".brand_recommend_more"));
            jse.ExecuteScript("arguments[0].click();", findMoreBtn);

            driver.FindElement(By.Id("search_brand")).Click();
            driver.FindElement(By.CssSelector("#search_brandlist li:nth-of-type(2)")).Click();
            driver.FindElement(By.Id("search_category")).Click();
            driver.FindElement(By.CssSelector("[search_category_img='drink.svg']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("#search_shopcardsbox a.card")).Click();

            orderProduct.OrderSomethingRandomOnCurrentMenu();

            Assert.AreEqual(driver.Url, "https://wowdin.azurewebsites.net/Member/Login");
        }

        // Check if the calculated amount in the shopping cart matches the amount sent to ECpay
        [Test]
        public void CreateOrder()
        {
            var shopId = 1;
            var couponId = 3;
            var couponProduct = "ªüÂÄ©i¬õ¯ù-¤j";

            login.LoginByEmail("tang.yen.chun@gmail.com", "tyc1234");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(".searchzone")));

            orderProduct.OrderSomethingHasDiscountOnSpecificMenu(shopId, couponProduct);
            orderProduct.GotoCartByClick();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);

            var priceToPayByCalculate = createOrder.SetCartAndCreateOrder(shopId, couponId);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("step3FinalPrice")));

            var finalPrice = driver.FindElement(By.Id("step3FinalPrice")).Text;
            var startIdx = finalPrice.IndexOf("$") + 1;
            var endIdx = finalPrice.IndexOf("¤¸");
            finalPrice = finalPrice.Substring(startIdx, endIdx - startIdx).Trim();

            Assert.AreEqual(priceToPayByCalculate, finalPrice);
        }

        //Check if the group buyer's product is listed in the shopping cart
        [Test]
        public void Group()
        {
            var shopId = 1;
            login.LoginByEmail("tang.yen.chun@gmail.com", "tyc1234");
            driver.Navigate().GoToUrl($"https://wowdin.azurewebsites.net/Menu/{shopId}");
            orderProduct.OrderSomethingRandomOnCurrentMenu();
            var groupUrl = orderProduct.GetGroupURL();

            jse.ExecuteScript($"window.open('https://wowdin.azurewebsites.net/Member/Login', 'newTab');");
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            Thread.Sleep(5000);
            login.LoginByEmail("molly@gmail.com", "molly1234");
            driver.Navigate().GoToUrl(groupUrl);
            orderProduct.OrderSomethingRandomOnCurrentMenu();

            driver.SwitchTo().Window(driver.WindowHandles[0]);
            login.LoginByEmail("tang.yen.chun@gmail.com", "tyc1234");
            driver.Navigate().GoToUrl($"https://wowdin.azurewebsites.net/Order/CartDetail/{shopId}");

            var users = driver.FindElements(By.Id("productListByUser"));

            Assert.AreEqual(2, users.Count);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
                driver.Quit();
        }
    }
}