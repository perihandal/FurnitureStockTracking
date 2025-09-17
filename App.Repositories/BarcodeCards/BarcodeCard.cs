using App.Repositories.StockCards;
using App.Repositories.Users;
using App.Repositories.Branches;
using App.Repositories.Companies;

namespace App.Repositories.BarcodeCards
{
    public enum BarcodeType
    {
        EAN13 = 1,
        UPC_A = 2,
        QRCode = 3,
        Code128 = 4,
        EAN8 = 5,
        ITF14 = 6,
        ISBN = 7,
        DataMatrix = 8
    }


    public class BarcodeCard
        {
            public int Id { get; set; }
            public string BarcodeCode { get; set; } = default!;  // Benzersiz Barkod Kodu
            public BarcodeType BarcodeType { get; set; }  // Barkod Tipi (enum olarak)
            public bool IsDefault { get; set; } = false;  // Varsayılan barkod mu?
            public bool IsActive  = true;  // Varsayılan barkod mu?

            public int StockCardId { get; set; }  // İlgili Stok Kartı
            public StockCard StockCard { get; set; } = default!;

            public int? UserId { get; set; }
            public User? User { get; set; } 

            public DateTime CreateDate { get; set; } = DateTime.UtcNow;  // Barkod oluşturulma tarihi

            public int BranchId { get; set; }  // Şube ID
            public Branch Branch { get; set; } = default!;  // Şube

            public int CompanyId { get; set; }  // Şirket ID
            public Company Company { get; set; } = default!; // Şirket
        }
    

}
