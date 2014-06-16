using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using Sample.Core.Models;
using Sample.Core.Security;

namespace Sample.Core.Test.Integration.Leads
{
    [TestFixture, Description("Lead Tests")]
    public class LeadsTests
    {
        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            //_tx = _context.Database.BeginTransaction();
            _userStore = new UserStore<User>(_context);
            _userManager = new UserManager<User>(_userStore)
            {
                UserValidator = new EmailUsernameIdentityValidator(),
                ClaimsIdentityFactory = new UserClaimsIdentityFactory()
            };
        }

        [TearDown]
        public void TearDown()
        {
            //_tx.Rollback();
            _userManager.Dispose();
            _context.Dispose();
        }

        private DataContext _context;
        private UserManager<User> _userManager;
        private UserStore<User> _userStore;
        private DbContextTransaction _tx;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        private Lead GetTestLead()
        {
            return new Lead
            {
                Email = "testuser@sample.com",
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

        [Test, Description("Creating, editing, deleting of a lead should work.")]
        public void CreateEditDeleteOfALeadShouldWork()
        {
            Lead lead = GetTestLead();
            string guid = Guid.NewGuid().ToString();
            lead.Email = String.Format("{0}@sample.com", guid);
            _context.Leads.AddOrUpdate(x => x.Email, lead);
            _context.SaveChanges();

            Lead savedLead = _context.Leads.FirstOrDefault(x => x.Email == lead.Email);
            Assert.AreEqual(savedLead.Email, lead.Email);

            _context.Leads.Remove(savedLead);
            _context.SaveChanges();
            Lead removedLead = _context.Leads.FirstOrDefault(x => x.Email == savedLead.Email);
            Assert.IsNull(removedLead);
        }

        [Test, Description("Validating a lead who does not have an email, phone, nor address should fail.")]
        public void ValidatingALeadWhoDoesNotHaveAnEmailOrPhoneOrAddressShouldFail()
        {
            var lead = new Lead
            {
                Email = "",
                FirstName = "Johnny",
                MiddleName = "J",
                LastName = "Lead",
                Phone = "",
                Description = "A solid lead",
                Address = null
            };
            IEnumerable<ValidationResult> validationResult = lead.Validate(new ValidationContext(lead, null, null));
            Assert.AreEqual(validationResult.Count(), 1);
            foreach (ValidationResult result in validationResult)
            {
                Assert.AreEqual(result.ErrorMessage,
                    "A Lead must have one of the following: a valid email, phone, or mailing address");
            }
        }

        [Test, Description("Validating a lead with an invalid email should fail.")]
        public void ValidatingALeadWithAnInvalidEmailShouldFail()
        {
            Lead lead = GetTestLead();
            lead.Email = "asd@asd.";

            IEnumerable<ValidationResult> validationResult = lead.Validate(new ValidationContext(lead, null, null));
            Assert.AreEqual(validationResult.Count(), 1);
            foreach (ValidationResult result in validationResult)
            {
                Assert.AreEqual(result.ErrorMessage, "Email is invalid: " + lead.Email);
            }

            // An apostrophe should fail because our regex does not allow it
            lead.Email = "asd'asda@asd.com";
            validationResult = lead.Validate(new ValidationContext(lead, null, null));
            Assert.AreEqual(validationResult.Count(), 1);
            foreach (ValidationResult result in validationResult)
            {
                Assert.AreEqual(result.ErrorMessage, "Email is invalid: " + lead.Email);
            }

            // Add more conditions
        }

        [Test, Description("Validating a lead should work.")]
        public void ValidatingAValidLeadShouldWork()
        {
            Lead lead = GetTestLead();
            IEnumerable<ValidationResult> validationResult = lead.Validate(new ValidationContext(lead, null, null));
            Assert.AreEqual(validationResult.Count(), 0);
        }
    }
}