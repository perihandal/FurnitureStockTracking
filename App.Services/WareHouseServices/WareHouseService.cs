using App.Repositories;
using App.Repositories.Companies;
using System.Net;
using App.Services.CompanyServices;
using App.Repositories.Warehouses;
using App.Repositories.Branches;
using App.Repositories.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace App.Services.WareHouseServices
{
    public class WareHouseService(IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork) : IWareHouseService
    {
        public async Task<ServiceResult<CreateWareHouseResponse>> CreateAsync(CreateWareHouseRequest request)
        {
            var warehouse = new Warehouse()
            {
                Code = request.Code,              // Stok kartı kodu
                Name = request.Name,              // Stok kartı adı
                Address = request.Address,        // Adres
                Phone = request.Phone,            // Telefon
                IsActive = true,      // Eğer `IsActive` request'ten geliyorsa, yoksa default olarak `true` olabilir
                CompanyId = request.CompanyId,    // Şirket ID'si
                BranchId = request.BranchId ,     // Şube ID'si
                UserId = request.UserId,
            };

            await warehouseRepository.AddAsync(warehouse);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateWareHouseResponse>.Success(new CreateWareHouseResponse(warehouse.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateWareHouseRequest request)
        {
            var warehouse = await warehouseRepository.GetByIdAsync(id);

            if (warehouse == null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            warehouse.Code = request.Code;
            warehouse.Name = request.Name;
            warehouse.Address = request.Address;
            warehouse.Phone = request.Phone;
            warehouse.IsActive = request.IsActive;
            warehouse.CompanyId = request.CompanyId;
            warehouse.BranchId = request.BranchId;
            warehouse.UserId = request.UserId;


            warehouseRepository.Update(warehouse);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);


        }

        public async Task<ServiceResult<List<WareHouseDto>>> GetAllList()
        {
            var warehouses = await warehouseRepository.GetAllWithDetailsAsync();

            var warehouseAsDto = warehouses.Select(c => new WareHouseDto(
            c.Name,
            c.Address,
            c.Phone,
            c.IsActive,
            c.Branch.Name,
            c.Company.Name
            )).ToList();

            return ServiceResult<List<WareHouseDto>>.Success(warehouseAsDto);
        }
    }
}
