using App.Repositories.Branches;
using App.Repositories.Warehouses;

namespace App.Services.WareHouseServices
{
    public record class WareHouseDto(
        int Id,
        string Code,
        string Name,                    // Depo adı
        string Address,                 // Adres
        string Phone,                   // Telefon
        bool IsActive,                  // Aktif/Pasif durumu
        int BranchId,
        string BranchName,
        int CompanyId,
        string CompanyName,            // Şirket adı
        int UserId,
        string UserFullName
    );


}

