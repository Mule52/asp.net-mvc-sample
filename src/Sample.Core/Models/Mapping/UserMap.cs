using System.Data.Entity.ModelConfiguration;

namespace Sample.Core.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Users");
            HasKey(t => t.Id);

            Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.SecurityStamp)
                .IsRequired()
                .HasMaxLength(255);

            // this.HasOptional(x => x.Organization).WithMany(x => x.Users).HasForeignKey(x => x.OrganizationId).WillCascadeOnDelete();
        }
    }
}