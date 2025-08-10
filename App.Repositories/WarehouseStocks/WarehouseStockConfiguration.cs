using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.WarehouseStocks
{
    public class WarehouseStockConfiguration : IEntityTypeConfiguration<WarehouseStock>
    {
        public void Configure(EntityTypeBuilder<WarehouseStock> builder)
        {
            builder.ToTable("WarehouseStocks");

            builder.HasKey(ws => ws.Id);

            builder.HasOne(ws => ws.Warehouse)
                   .WithMany(w => w.WarehouseStocks)
                   .HasForeignKey(ws => ws.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ws => ws.StockCard)
                   .WithMany(sc => sc.WarehouseStocks)
                   .HasForeignKey(ws => ws.StockCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ws => ws.Quantity)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
