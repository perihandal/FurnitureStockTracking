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

            // Enum'ı string olarak veritabanına dönüştürme
            builder.Property(st => st.Type)
                   .IsRequired()
                   .HasMaxLength(20) // Enum'un veritabanında saklanacağı maksimum uzunluk
                   .HasConversion(
                       v => v.ToString(), // Enum değerini string'e dönüştür
                       v => (TransactionType)Enum.Parse(typeof(TransactionType), v) // String'i Enum'a dönüştür
                   );

            builder.Property(st => st.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(st => st.TransactionDate)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(st => st.DocumentNumber)
                   .HasMaxLength(50);

            builder.HasIndex(bc => bc.DocumentNumber)
               .IsUnique();

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

            builder.HasOne(st => st.FromWarehouse)
                   .WithMany()
                   .HasForeignKey(st => st.FromWarehouseId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); // Giriş ve Çıkış için nullable olabilir

            builder.HasOne(st => st.ToWarehouse)
                   .WithMany()
                   .HasForeignKey(st => st.ToWarehouseId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); // Transfer için nullable olabilir

            builder.HasOne(sc => sc.User)
                   .WithMany(s=>s.StockTransactions)
                   .HasForeignKey(sc => sc.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
