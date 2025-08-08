using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.PriceDefinitions
{
    public class PriceDefinitionConfiguration : IEntityTypeConfiguration<PriceDefinition>
    {
        public void Configure(EntityTypeBuilder<PriceDefinition> builder)
        {
            builder.ToTable("PriceDefinitions");

            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.PriceType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(pd => pd.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(pd => pd.Currency)
                .IsRequired();

            builder.Property(pd => pd.ValidFrom)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(pd => pd.ValidTo)
                   .HasColumnType("datetime");

            builder.Property(pd => pd.IsActive)
                .HasDefaultValue(false);  // Varsayılan olarak false



            builder.HasOne(pd => pd.StockCard)
                   .WithMany(sc => sc.PriceDefinitions)
                   .HasForeignKey(pd => pd.StockCardId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.User)
                   .WithMany(p => p.PriceDefinitions)
                   .HasForeignKey(sc => sc.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pd => new { pd.StockCardId, pd.PriceType })
                   .IsUnique(); // Aynı StockCardId ve PriceType ile aynı fiyatın olmasını engeller
        }
    }
    }

