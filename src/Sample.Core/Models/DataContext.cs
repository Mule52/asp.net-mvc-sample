using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Sample.Core.Models.Mapping;

namespace Sample.Core.Models
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext()
            : base("SampleWebManagement")
        {
        }

        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());

            modelBuilder.Entity<IdentityUser>()
                .HasKey(t => t.Id)
                .ToTable("Users")
                .Property(t => t.UserName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<IdentityRole>()
                .HasKey(t => t.Id)
                .ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(t => new { t.UserId, t.RoleId })
                .ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserClaim>()
                .HasKey(t => t.Id)
                .ToTable("UserClaims");

            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("ProviderUserLogins");

            modelBuilder.Entity<Lead>()
                .HasKey(t => t.Id)
                .ToTable("Leads");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}