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

            builder.HasIndex(bc => bc.Code)
               .IsUnique();

            builder.Property(sg => sg.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(sg => sg.IsActive)
                   .HasDefaultValue(true);

            builder.HasOne(sg => sg.MainGroup)
                   .WithMany(mg => mg.SubGroups)  // Bir MainGroup birden fazla SubGroup'a sahip
                   .HasForeignKey(sg => sg.MainGroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sg => sg.User)
                   .WithMany(s =>s.SubGroups)
                   .HasForeignKey(sg => sg.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}