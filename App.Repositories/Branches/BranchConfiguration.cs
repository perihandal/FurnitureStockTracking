using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Repositories.BarcodeCards;
using App.Repositories.Companies;
using App.Repositories.StockCards;
using App.Repositories.Users;
using App.Repositories.Warehouses;

namespace App.Repositories.Branches
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            // Code ve Name alanlarını zorunlu yapıyoruz
            builder.Property(b => b.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(bc => bc.Code)
                .IsUnique(); 

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true); // Varsayılan olarak aktif

            // Foreign Key: Company ile ilişki
            builder.HasOne(b => b.Company)  // Bir şube bir şirkete bağlıdır
                .WithMany(c => c.Branches)  // Bir şirketin birden fazla şubesi olabilir
                .HasForeignKey(b => b.CompanyId)  // Şube için yabancı anahtar CompanyId
                .OnDelete(DeleteBehavior.Restrict);  // Şirket silindiğinde şube silinmesin

            // Foreign Key: User ile ilişki
            builder.HasOne(b => b.User)  // Bir şube bir kullanıcıya bağlıdır
                .WithMany(b=>b.Branches)  // Bir kullanıcının birden fazla şubesi olabilir
                .HasForeignKey(b => b.UserId)  // Şube için yabancı anahtar UserId
                .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silindiğinde şube silinmesin

            // Şube ile ilgili stok kartları
            builder.HasMany(b => b.StockCards)  // Şube birden fazla StockCard'a sahip olabilir
                .WithOne(sc => sc.Branch)  // Her StockCard bir şubeye ait olmalıdır
                .HasForeignKey(sc => sc.BranchId)  // StockCard'ın şube ile olan ilişkiyi belirtir
                .OnDelete(DeleteBehavior.Cascade);  // Şube silindiğinde ilişkili StockCard'lar silinsin

            // Şube ile ilgili depo bilgileri
            builder.HasMany(b => b.Warehouses)  // Şube birden fazla Warehouse'a sahip olabilir
                .WithOne(w => w.Branch)  // Her Warehouse bir şubeye ait olmalıdır
                .HasForeignKey(w => w.BranchId)  // Warehouse'ın şube ile olan ilişkiyi belirtir
                .OnDelete(DeleteBehavior.Cascade);  // Şube silindiğinde ilişkili Warehouse'lar silinsin

            // Şube ile ilgili barkod kartları
            builder.HasMany(b => b.BarcodeCards)  // Şube birden fazla BarcodeCard'a sahip olabilir
                .WithOne(bc => bc.Branch)  // Her BarcodeCard bir şubeye ait olmalıdır
                .HasForeignKey(bc => bc.BranchId)  // BarcodeCard'ın şube ile olan ilişkiyi belirtir
                .OnDelete(DeleteBehavior.Cascade);  // Şube silindiğinde ilişkili BarcodeCard'lar silinsin
        }
    }
}
