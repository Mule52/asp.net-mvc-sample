using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using Sample.Core.Models;
using Sample.Core.Security;

namespace Sample.Core.Test.Integration.Users
{
    [TestFixture, Description("User Tests")]
    public class UserTests
    {
        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _tx = _context.Database.BeginTransaction();
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
            _tx.Rollback();
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

        [Test, Rollback, Description("User creation and get the user account id.")]
        public void CreateUserAndAccountReturnsNewAccount()
        {
            var user = GetTestUser();
            IdentityResult res = _userManager.Create(user, "hello!!");
            _context.SaveChanges();
            Assert.IsTrue(res.Succeeded);
            Assert.IsNotNull(user.Id);
        }

        [Test, Description("User creation.")]
        public void CreateEditDeleteUserShouldWorkAsExpected()
        {
            var guid = Guid.NewGuid().ToString();
            string email = "asdasdadasasas@sample.com";
            var user = new User
            {
                UserName = email,
                FirstName = "Test",
                LastName = "TestUser",
                Organization = null,
                Email = email,
                PasswordHash = "AGDf9Gb2ZHueGTd9a/TD1/B8QXD8TVmt8RAwW97DDu5JPst8zNwzo2tbOQoijC3P5w==",
                SecurityStamp = "002a0401-d6df-4f12-b716-42c6e087b49d"
            };

            Assert.AreEqual(user.UserName, email);
            Assert.AreEqual(user.FirstName, "Test");
            Assert.AreEqual(user.LastName, "TestUser");

            user.FirstName = "FnameTestUser";
            user.LastName = "LnameTestUser";
            _context.Users.AddOrUpdate(x => x.UserName, user);
            _context.SaveChanges();

            var newUser = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            Assert.AreEqual(newUser.UserName, email);
            Assert.AreEqual(newUser.FirstName, "FnameTestUser");
            Assert.AreEqual(newUser.LastName, "LnameTestUser");

            _context.Users.Remove(newUser);
            _context.SaveChanges();

            var deletedUser = _context.Users.FirstOrDefault(x => x.UserName == newUser.UserName);
            Assert.IsNull(deletedUser);
        }

        private User GetTestUser()
        {
            return new User
            {
                UserName = "testuser@sample.com",
                FirstName = "Test",
                LastName = "TestUser"
            };
        }

    }
}