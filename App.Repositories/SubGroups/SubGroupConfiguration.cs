using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.SubGroups
{
    public class SubGroupConfiguration : IEntityTypeConfiguration<SubGroup>
    {
        public void Configure(EntityTypeBuilder<SubGroup> builder)
        {
            builder.ToTable("SubGroups");

            builder.HasKey(sg => sg.Id);

            builder.Property(sg => sg.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(sg => sg.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(sg => sg.IsActive)
                   .HasDefaultValue(true);

            builder.HasOne(sg => sg.MainGroup);

        }
    }
}