using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Sample.Core.Models.Mapping;

namespace Sample.Core.Models
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext()
            : base("Sample")
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        //protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        //{
        //    var entity = entityEntry.Entity as IAuditable;
        //    if (entity == null) return base.ValidateEntity(entityEntry, items);
        //    if (entityEntry.State == EntityState.Added)
        //    {
        //        entity.CreateDate = DateTime.UtcNow;
        //        entity.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
        //        entity.ModifiedDate = DateTime.UtcNow;
        //        entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
        //    }

        //    if (entityEntry.State == EntityState.Modified)
        //    {
        //        if (entity.CreateDate == null || entity.CreateDate == DateTime.MinValue)
        //            entity.CreateDate = DateTime.UtcNow;
        //        entity.ModifiedDate = DateTime.UtcNow;
        //        entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
        //    }
        //    return base.ValidateEntity(entityEntry, items);
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            //modelBuilder.Configurations.Add(new PasswordResetTokenMap());

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


            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("InstructorID")
                    .ToTable("CourseInstructor"));
            modelBuilder.Entity<Department>().MapToStoredProcedures();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add(new UserMap());
            //modelBuilder.Configurations.Add(new RoleMap());
            //modelBuilder.Configurations.Add(new PasswordResetTokenMap());

            //modelBuilder.Entity<IdentityUser>()
            //    .HasKey(t => t.Id)
            //    .ToTable("Users")
            //    .Property(t => t.UserName)
            //    .HasMaxLength(255)
            //    .IsRequired();

            //modelBuilder.Entity<IdentityRole>()
            //    .HasKey(t => t.Id)
            //    .ToTable("Roles");

            //modelBuilder.Entity<IdentityUserRole>()
            //    .HasKey(t => new { t.UserId, t.RoleId })
            //    .ToTable("UserRoles");

            //modelBuilder.Entity<IdentityUserClaim>()
            //    .HasKey(t => t.Id)
            //    .ToTable("UserClaims");

            //modelBuilder.Entity<IdentityUserLogin>()
            //    .ToTable("ProviderUserLogins");
        }

        //public override int SaveChanges()
        //{
        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable)
        //        {
        //            // 0, 1, or N. This is the 1 case. Refactor for N when N appears.
        //            if (entry.Entity is User)
        //            {
        //                // When we delete a user, we set the DisabledDateTime = DateTime.Now.
        //                // This will null out the IdentityUser_Id in UserRoles and keep the
        //                // User from being permanently deleted in the database.
        //                var user = entry.Entity as User;
        //                user.DisabledDateTime = DateTime.Now;

        //                // Ensure organizationdId is not set to null due to the delete
        //                user.OrganizationId = entry.OriginalValues.GetValue<long>("OrganizationId");

        //                // Change the entry.State to be Modified instead of Delete.
        //                entry.State = EntityState.Modified;
        //            }
        //        }
        //    }
        //    return base.SaveChanges();
        //}
    }
}