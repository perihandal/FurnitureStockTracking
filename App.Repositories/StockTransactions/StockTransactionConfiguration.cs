using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockTransactions
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Change).IsRequired();
            builder.Property(s => s.Description).IsRequired().HasMaxLength(200);
            builder.Property(s => s.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(s => s.Product)
                   .WithMany(p => p.StockTransactions)
                   .HasForeignKey(s => s.ProductId);

            builder.HasOne(s => s.CreatedByUser)
                   .WithMany(u => u.StockTransactions)
                   .HasForeignKey(s => s.CreatedByUserId);
        }
    }

}
