using NUnit.Framework;
using OpenQA.Selenium;
using Sample.Web.Test.Automation.Common;

namespace Sample.Web.Test.Automation.LeadTests
{
    [TestFixture, Description("Leads Test")]
    internal class LeadTests : BaseTest
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Security.AuthenticateAsAutomationUser();

            browser.Navigate().GoToUrl(Url.DomainPrepath + "/Lead");
            browser.WaitUntilClickable(By.LinkText("Leads")).Click();
        }

        [Test, Description("Navigating to the Leads page should work as expected.")]
        public void NavigatingToTheLeadsPageShouldWork()
        {
            // TODO: start testing
            // At this point, we're on the Leads page looking at the first 10 leads.
            Assert.IsTrue(true);
        }
    }
}