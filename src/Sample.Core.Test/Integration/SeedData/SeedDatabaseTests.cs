using System.Data.Entity;
using System.Data.Entity.Migrations;
using NUnit.Framework;
using Sample.Core.Migrations;
using Sample.Core.Models;

namespace Sample.Core.Test.Integration.SeedData
{
    [TestFixture, Description("Database Seed Tests")]
    public class SeedDatabaseTests
    {
        protected DataContext context;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            context = new DataContext();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            context.Dispose();
        }

        /// <summary>
        ///     Drop the database if it exists so we can update it from seed. Sometimes
        ///     users still have connects open, so alter the database to allow deleting.
        /// </summary>
        private void DeleteDatabaseIfExists()
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "ALTER DATABASE [" + context.Database.Connection.Database +
                    "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                context.Database.Delete();
            }
        }

        [Test, Description("Creating and Seed database.")]
        public void CreateAndSeedDatabase()
        {
            // If you need to physically delete the database first, uncomment below.
            //DeleteDatabaseIfExists();
            var configuration = new Configuration();
            configuration.AutomaticMigrationDataLossAllowed = false;
            var migrator = new DbMigrator(configuration);
            migrator.Update("0");
            migrator.Update();
        }

        [Test, Description("Updating and seed database for production")]
        public void CreateAndSeedDatabaseForProduction()
        {
            var config = new Configuration();
            config.AutomaticMigrationDataLossAllowed = true;
            var migrator = new DbMigrator(config);
            migrator.Update("0");
            migrator.Update();
        }

        [Test, Description("Updating and Seed database.")]
        public void UpdateAndSeedDatabase()
        {
            var configuration = new Configuration();
            configuration.AutomaticMigrationDataLossAllowed = true;
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}