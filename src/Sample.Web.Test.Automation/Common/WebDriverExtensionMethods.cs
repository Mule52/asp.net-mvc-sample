using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Sample.Web.Test.Automation.Common
{
    public static class WebDriverExtensionMethods
    {
        public static IWebElement WaitUntilClickable(this IWebDriver webDriver, By by, int seconds)
        {
            var wait = new WebDriverWait(AutofacConfig.WebDriver(), TimeSpan.FromSeconds(seconds));
            return wait.Until(ExpectedConditions.ElementIsClickable(by));
        }

        public static IWebElement WaitUntilClickable(this IWebDriver webDriver, By by)
        {
            return WaitUntilClickable(webDriver, by, 10);
        }
    }
}