using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Repositories
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Type)
                   .IsRequired()
                   .HasConversion<string>(); // Enum ise string olarak saklar

            builder.Property(p => p.Unit)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.StockQuantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.Property(p => p.CreatedDate)
                   .HasColumnType("datetime") // datetime(6) yerine sadece datetime
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}

