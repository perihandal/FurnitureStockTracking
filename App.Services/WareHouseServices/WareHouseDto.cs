using App.Repositories.Branches;
using App.Repositories.Warehouses;

namespace App.Services.WareHouseServices
{
    public record class WareHouseDto(
         string Name,                    // Depo adı
        string Address,                 // Adres
        string Phone,                   // Telefon
        bool IsActive,                  // Aktif/Pasif durumu
        string BranchName,              // Şube adı
        string CompanyName            // Şirket adı
    );


}

