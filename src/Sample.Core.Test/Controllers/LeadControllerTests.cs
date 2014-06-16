using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using Sample.Core.Models;
using Sample.Web.Controllers;

namespace Sample.Core.Test.Controllers
{
    [TestFixture, Description("LeadController Tests")]
    public class LeadControllerTests
    {
        [SetUp]
        protected virtual void SetUp()
        {
            _context = new DataContext();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            try
            {
                _context.Dispose();
            }
            finally
            {
                _context = null;
            }
        }

        private DataContext _context;
        private UserManager<User> _userManager;
        private UserStore<User> _userStore;
        private DbContextTransaction _tx;

        private Lead GetTestLead()
        {
            return new Lead
            {
                Email = "leadcontrollertest@sample.com",
                FirstName = "Johnny",
                MiddleName = "J",
                LastName = "Lead",
                Phone = "222-333-4444",
                Description = "A solid lead",
                Address = new Address
                {
                    Street = "123 Main St.",
                    City = "Boise",
                    State = "ID",
                    Zip = "83702",
                    Country = "USA"
                }
            };
        }

        [Test, Description("Read unassigned message.")]
        public void CreateEditDeleteShouldCreateUpdateAndDeleteALead()
        {
            Lead lead = GetTestLead();

            using (var controller = new LeadController(_context))
            {
                var result = (RedirectToRouteResult) controller.Create(lead);
                Assert.IsTrue(result.RouteValues.ContainsKey("action"));
                Assert.AreEqual("Index", result.RouteValues["action"].ToString());

                Lead savedLead = _context.Leads.FirstOrDefault(x => x.Email == lead.Email);
                var editResult = (RedirectToRouteResult) controller.Edit(savedLead);
                Assert.IsTrue(editResult.RouteValues.ContainsKey("action"));
                Assert.AreEqual("Index", editResult.RouteValues["action"].ToString());

                Lead updatedLead = _context.Leads.FirstOrDefault(x => x.Email == lead.Email);
                var deleteResult = (RedirectToRouteResult) controller.Delete(updatedLead.Id);
                Assert.IsTrue(deleteResult.RouteValues.ContainsKey("action"));
                Assert.AreEqual("Index", deleteResult.RouteValues["action"].ToString());

                Lead deletedLead = _context.Leads.FirstOrDefault(x => x.Email == lead.Email);
                Assert.IsNull(deletedLead);
            }
        }
    }
}