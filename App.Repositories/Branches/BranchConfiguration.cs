﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Branches
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.Address)
                   .HasMaxLength(500);

            builder.Property(b => b.Phone)
                   .HasMaxLength(20);

            builder.Property(b => b.IsActive)
                   .HasDefaultValue(true);

            builder.HasOne(b => b.Company)
                   .WithMany(c => c.Branches)
                   .HasForeignKey(b => b.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.StockCards)
                   .WithOne(s => s.Branch)
                   .HasForeignKey(s => s.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Warehouses)
                   .WithOne(w => w.Branch)
                   .HasForeignKey(w => w.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
