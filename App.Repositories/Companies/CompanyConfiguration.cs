using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Companies
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.TaxNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.Address)
                   .HasMaxLength(500);

            builder.Property(c => c.Phone)
                   .HasMaxLength(20);

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            builder.HasMany(c => c.Branches)
                   .WithOne(b => b.Company)
                   .HasForeignKey(b => b.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.StockCards)
                   .WithOne(s => s.Company)
                   .HasForeignKey(s => s.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Warehouses)
                   .WithOne(w => w.Company)
                   .HasForeignKey(w => w.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
