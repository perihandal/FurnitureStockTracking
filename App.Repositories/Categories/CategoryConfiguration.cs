using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Categories
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(bc => bc.Code)
               .IsUnique();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);


            builder.Property(c => c.CreateDate)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.BranchId)
                   .IsRequired();

            builder.Property(c => c.CompanyId)
                   .IsRequired();

            builder.HasMany(c => c.StockCards)
                   .WithOne(sc => sc.Category)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Branch)
                   .WithMany()
                   .HasForeignKey(c => c.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Company)
                   .WithMany()
                   .HasForeignKey(c => c.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User)
                   .WithMany(cg => cg.Categories)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
