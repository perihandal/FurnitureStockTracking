using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Warehouses
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(bc => bc.Code)
               .IsUnique();

            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(w => w.Address)
                   .HasMaxLength(500);
            
            builder.Property(w => w.Phone)
                   .HasMaxLength(500);


            builder.Property(w => w.IsActive)
                   .HasDefaultValue(true);

            builder.HasOne(w => w.Company)
                   .WithMany(c => c.Warehouses)
                   .HasForeignKey(w => w.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Branch)
                   .WithMany(b => b.Warehouses)
                   .HasForeignKey(w => w.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sc => sc.User)
                   .WithMany(w=> w.Warehouses)
                   .HasForeignKey(sc => sc.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
