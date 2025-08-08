using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.MainGroups
{
    public class MainGroupConfiguration : IEntityTypeConfiguration<MainGroup>
    {
        public void Configure(EntityTypeBuilder<MainGroup> builder)
        {
            builder.ToTable("MainGroups");

            builder.HasKey(mg => mg.Id);

            builder.Property(mg => mg.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(bc => bc.Code)
               .IsUnique();

            builder.Property(mg => mg.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(mg => mg.IsActive)
                   .HasDefaultValue(true);

            builder.HasMany(mg => mg.SubGroups)
                   .WithOne(sg => sg.MainGroup)
                   .HasForeignKey(sg => sg.MainGroupId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(mg => mg.StockCards)
                   .WithOne(sc => sc.MainGroup)
                   .HasForeignKey(sc => sc.MainGroupId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mg=> mg.User)
                   .WithMany(m=>m.MainGroups)
                   .HasForeignKey(mg => mg.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
