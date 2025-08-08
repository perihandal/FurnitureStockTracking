using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.PriceHistories
{
    public class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
    {
        public void Configure(EntityTypeBuilder<PriceHistory> builder)
        {
            builder.ToTable("PriceHistories");

            builder.HasKey(ph => ph.Id);

            builder.Property(ph => ph.PriceType)
                .IsRequired();

            builder.Property(ph => ph.OldPrice)
                .IsRequired();

            builder.Property(ph => ph.NewPrice)
                .IsRequired();

            builder.Property(ph => ph.ChangeDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(ph => ph.PriceDefinition) // PriceHistory ile PriceDefinition arasındaki ilişkiyi tanımlıyoruz
                .WithMany()  // PriceDefinition'ın birden fazla PriceHistory'ye sahip olabileceğini belirtiyoruz
                .HasForeignKey(ph => ph.PriceDefinitionId)  // PriceHistory'deki PriceDefinitionId'yi PriceDefinition ile ilişkilendiriyoruz
                .OnDelete(DeleteBehavior.Restrict);  // PriceDefinition silindiğinde PriceHistory'yi silmiyoruz
        }
    }
}
