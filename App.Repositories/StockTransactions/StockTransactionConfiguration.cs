using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.StockTransactions
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.TransactionType)
                   .IsRequired()
                   .HasMaxLength(20); // "Giris", "Cikis", "Transfer" gibi tipler olacak.

            builder.Property(st => st.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(st => st.TransactionDate)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(st => st.DocumentNumber)
                   .HasMaxLength(50);

            builder.Property(st => st.Description)
                   .HasMaxLength(500);

            builder.HasOne(st => st.StockCard)
                   .WithMany(sc => sc.StockTransactions)
                   .HasForeignKey(st => st.StockCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(st => st.Warehouse)
                   .WithMany()
                   .HasForeignKey(st => st.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
