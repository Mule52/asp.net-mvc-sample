using NUnit.Framework;
using OpenQA.Selenium;
using Sample.Web.Test.Automation.Common;

namespace Sample.Web.Test.Automation.LeadTests
{
    [TestFixture, Description("User Administration Test")]
    internal class UserTests : BaseTest
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Security.AuthenticateAsAutomationUser();
            browser.WaitUntilClickable(By.LinkText("automation@sample.com")).Click();
        }

        [Test, Description("Navigating to the User administration page should work as expected.")]
        public void NavigatingToTheUserAminPageShouldWork()
        {
            // TODO: start testing
            // At this point, we're on the User admin page
            Assert.IsTrue(true);
        }
    }
}