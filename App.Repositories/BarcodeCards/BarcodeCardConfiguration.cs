using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.BarcodeCards
{
    public class BarcodeCardConfiguration : IEntityTypeConfiguration<BarcodeCard>
    {
        public void Configure(EntityTypeBuilder<BarcodeCard> builder)
        {
            builder.ToTable("BarcodeCards");

            builder.HasKey(bc => bc.Id);

            builder.Property(bc => bc.Barcode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(bc => bc.BarcodeType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(bc => bc.IsDefault)
                   .HasDefaultValue(false);

            builder.HasOne(bc => bc.StockCard)
                   .WithMany(sc => sc.BarcodeCards)
                   .HasForeignKey(bc => bc.StockCardId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
