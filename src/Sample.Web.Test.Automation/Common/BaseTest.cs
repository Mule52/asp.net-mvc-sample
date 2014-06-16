using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Sample.Web.Test.Automation.Common;

namespace Sample.Web.Test.Automation
{
    internal class BaseTest
    {
        // it does not override if it exists
        protected IWebDriver browser;
        protected Random random = new Random();

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            AutofacConfig.Bootstrap();
            browser = AutofacConfig.WebDriver();
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            browser.Quit();
            browser.Dispose();
        }

        [SetUp]
        public virtual void Setup()
        {
            browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            browser.Manage().Window.Maximize();
        }

        [TearDown]
        public void Teardown()
        {
        }
    }

    public static class ExpectedConditions
    {
        public static Func<IWebDriver, IWebElement> ElementIsClickable(By by)
        {
            return driver =>
            {
                IWebElement element = driver.FindElement(by);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }
    }
}