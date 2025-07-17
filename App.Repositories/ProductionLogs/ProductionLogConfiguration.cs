using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.ProductionLogs
{
    public class ProductionLogConfiguration : IEntityTypeConfiguration<ProductionLog>
    {
        public void Configure(EntityTypeBuilder<ProductionLog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(p => p.Product)
                   .WithMany(p => p.ProductionLogs)
                   .HasForeignKey(p => p.ProductId);

            builder.HasOne(p => p.CreatedByUser)
                   .WithMany(u => u.ProductionLogs)
                   .HasForeignKey(p => p.CreatedByUserId);
        }
    }

}
