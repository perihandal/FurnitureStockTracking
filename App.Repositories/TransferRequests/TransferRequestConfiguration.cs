using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.TransferRequests
{
    public class TransferRequestConfiguration : IEntityTypeConfiguration<TransferRequest>
    {
        public void Configure(EntityTypeBuilder<TransferRequest> builder)
        {
            builder.ToTable("TransferRequests");

            builder.HasKey(tr => tr.Id);
            builder.Property(tr => tr.Id).ValueGeneratedOnAdd();

            builder.Property(tr => tr.Quantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(tr => tr.Description)
                .HasMaxLength(500);

            builder.Property(tr => tr.ApprovalNotes)
                .HasMaxLength(500);

            builder.Property(tr => tr.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(tr => tr.RequestedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(tr => tr.ApprovedDate)
                .HasColumnType("datetime");

            builder.Property(tr => tr.CompletedDate)
                .HasColumnType("datetime");

            builder.Property(tr => tr.IsActive)
                .HasDefaultValue(true);

            // İlişkiler
            builder.HasOne(tr => tr.FromWarehouse)
                .WithMany()
                .HasForeignKey(tr => tr.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.ToWarehouse)
                .WithMany()
                .HasForeignKey(tr => tr.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.StockCard)
                .WithMany()
                .HasForeignKey(tr => tr.StockCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.RequestedByUser)
                .WithMany()
                .HasForeignKey(tr => tr.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.ApprovedByUser)
                .WithMany()
                .HasForeignKey(tr => tr.ApprovedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Index'ler
            builder.HasIndex(tr => tr.Status);
            builder.HasIndex(tr => tr.RequestedDate);
            builder.HasIndex(tr => new { tr.FromWarehouseId, tr.ToWarehouseId });
        }
    }
}