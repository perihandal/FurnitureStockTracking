using App.Repositories;
using App.Repositories.Companies;
using System.Net;
using App.Services.CompanyServices;
using App.Repositories.Warehouses;
using App.Repositories.Branches;
using App.Repositories.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace App.Services.WareHouseServices
{
    public class WareHouseService : BaseService, IWareHouseService
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IUnitOfWork unitOfWork;

        public WareHouseService(IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
            this.warehouseRepository = warehouseRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<CreateWareHouseResponse>> CreateAsync(CreateWareHouseRequest request)
        {
            // User rolü create işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult<CreateWareHouseResponse>.Fail("Depo oluşturma yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // CompanyId ve BranchId doğrulaması
            var accessValidation = ValidateEntityAccess(request.CompanyId, request.BranchId);
            if (!accessValidation.IsSuccess)
            {
                return ServiceResult<CreateWareHouseResponse>.Fail(accessValidation.ErrorMessage!, accessValidation.Status);
            }

            var warehouse = new Warehouse()
            {
                Code = request.Code,              
                Name = request.Name,              
                Address = request.Address,        
                Phone = request.Phone,            
                IsActive = request.IsActive,      
                CompanyId = request.CompanyId,    
                BranchId = request.BranchId,     
                UserId = request.UserId,
            };

            await warehouseRepository.AddAsync(warehouse);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateWareHouseResponse>.Success(new CreateWareHouseResponse(warehouse.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateWareHouseRequest request)
        {
            // User rolü update işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("Depo güncelleme yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            var warehouse = await warehouseRepository.GetByIdAsync(id);
            if (warehouse == null)
            {
                return ServiceResult.Fail("Warehouse not found", HttpStatusCode.NotFound);
            }

            // Mevcut entity'ye erişim kontrolü
            if (!CanAccessEntity(warehouse.CompanyId, warehouse.BranchId))
            {
                return ServiceResult.Fail("Bu depoya erişim yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Yeni veriler için erişim kontrolü
            var accessValidation = ValidateEntityAccess(request.CompanyId, request.BranchId);
            if (!accessValidation.IsSuccess)
            {
                return ServiceResult.Fail(accessValidation.ErrorMessage!, accessValidation.Status);
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

            // Admin değilse, sadece kendi company/branch verilerine erişebilir
            if (!IsAdmin())
            {
                var userCompanyId = GetUserCompanyId();
                var userBranchId = GetUserBranchId();

                if (userCompanyId.HasValue)
                {
                    warehouses = warehouses.Where(w => w.CompanyId == userCompanyId.Value).ToList();
                }

                // User rolü için branch kontrolü
                if (IsUser() && userBranchId.HasValue)
                {
                    warehouses = warehouses.Where(w => w.BranchId == userBranchId.Value).ToList();
                }
            }

            var warehouseAsDto = warehouses.Select(c => new WareHouseDto(
            c.Id,
            c.Code,
            c.Name,
            c.Address,
            c.Phone,
            c.IsActive,
            c.BranchId,
            c.Branch.Name,
            c.CompanyId,
            c.Company.Name,
            c.UserId ?? 0,
            c.User?.FullName ?? ""
            )).ToList();

            return ServiceResult<List<WareHouseDto>>.Success(warehouseAsDto);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            // User rolü delete işlemi yapamaz, sadece Admin delete yapabilir
            if (!IsAdmin())
            {
                return ServiceResult.Fail("Depo silme yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Warehouse'u ilişkili navigation property'leri ile birlikte getir
            var warehouse = await warehouseRepository.Where(w => w.Id == id)
                .Include(w => w.WarehouseStocks)
                .FirstOrDefaultAsync();

            if (warehouse == null)
                return ServiceResult.Fail("Warehouse not found", HttpStatusCode.NotFound);

            // Entity'ye erişim kontrolü (Admin olsa da şirket bazlı kontrol yapabiliriz)
            if (!CanAccessEntity(warehouse.CompanyId, warehouse.BranchId))
            {
                return ServiceResult.Fail("Bu depoya erişim yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Warehouse soft delete
            warehouse.IsActive = false;

            // WarehouseStocks için IsActive field yoksa sadece warehouse'u soft delete yapıyoruz
            // İleride WarehouseStock modeline IsActive eklendiyse aşağıdaki kod açılabilir:
            /*
            if (warehouse.WarehouseStocks != null && warehouse.WarehouseStocks.Any())
            {
                foreach (var ws in warehouse.WarehouseStocks)
                {
                    ws.IsActive = false;
                }
            }
            */

            warehouseRepository.Update(warehouse);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

    }
}
