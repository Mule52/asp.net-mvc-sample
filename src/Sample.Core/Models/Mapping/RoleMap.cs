using System.Data.Entity.ModelConfiguration;

namespace Sample.Core.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Roles");
            Property(r => r.Name)
                .HasMaxLength(256);
        }
    }
}