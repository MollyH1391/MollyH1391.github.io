using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestProject.Extensions
{
    internal static class Extensions
    {
        public static void MoveAndClickByJS(this IJavaScriptExecutor jse, IWebElement element)
        {
            jse.ExecuteScript($"scroll(0, {element.Location.Y - 100})");
            jse.ExecuteScript("arguments[0].click();", element);
        }
    }
}
