using OpenQA.Selenium;

namespace Sample.Web.Test.Automation.Common
{
    public class Security
    {
        private static void AuthenticateAs(string prepath, string sendKeys)
        {
            IWebDriver browser = AutofacConfig.WebDriver();
            browser.Navigate().GoToUrl(prepath + "/account/login");
            IWebElement username = browser.FindElement(By.Id("Email"));
            IWebElement password = browser.FindElement(By.Id("Password"));
            username.SendKeys(sendKeys);
            password.SendKeys("test!!");
            password.Submit();
        }

        public static void AuthenticateAsAutomationUser()
        {
            AuthenticateAs(Url.DomainPrepath, "automation@" + Url.TestDomain);
        }
    }
}