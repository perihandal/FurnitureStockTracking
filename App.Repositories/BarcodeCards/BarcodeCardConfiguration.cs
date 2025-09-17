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

            builder.Property(bc => bc.BarcodeCode)
                .IsRequired()  // Barkod kodu zorunlu olmalı
                .HasMaxLength(100);  // Maksimum uzunluk

            // Benzersiz indeks oluşturuluyor
            builder.HasIndex(bc => bc.BarcodeCode)
                .IsUnique();  // BarcodeCode benzersiz olacak

            builder.Property(bc => bc.BarcodeType)
            .IsRequired()
            .HasConversion<int>();

            builder.Property(bc => bc.IsDefault)
                .HasDefaultValue(false);

            builder.Property(bc => bc.IsActive)
                .HasDefaultValue(false);

            builder.Property(bc => bc.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // `StockCard` ile ilişki
            builder.HasOne(bc => bc.StockCard)
                .WithMany(sc => sc.BarcodeCards)
                .HasForeignKey(bc => bc.StockCardId)
                .OnDelete(DeleteBehavior.Cascade);

            // `User` ile ilişki
            builder.HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // `Branch` ile ilişki
            builder.HasOne(bc => bc.Branch)
                .WithMany(b => b.BarcodeCards)  // `Branch`'ın birden fazla `BarcodeCard`'a sahip olduğunu belirtiyoruz
                .HasForeignKey(bc => bc.BranchId)
                .OnDelete(DeleteBehavior.Restrict);  // Şube silindiğinde Barkod silinmesin

            // `Company` ile ilişki
            builder.HasOne(bc => bc.Company)
                .WithMany(c => c.BarcodeCards)  // `Company`'nin birden fazla `BarcodeCard`'a sahip olduğunu belirtiyoruz
                .HasForeignKey(bc => bc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);  // Şirket silindiğinde Barkod silinmesin
        }
    }
}
