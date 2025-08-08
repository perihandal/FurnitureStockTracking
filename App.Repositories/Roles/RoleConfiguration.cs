using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasMany(r => r.UserRoles)     // Role -> UserRoles (çoklu)
                   .WithOne(ur => ur.Role)        // UserRole -> Role (tekil)
                   .HasForeignKey(ur => ur.RoleId);
        }
    }
}
