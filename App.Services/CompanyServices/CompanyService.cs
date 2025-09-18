using App.Repositories.StockCards;
using App.Repositories;
using App.Services.StockCardServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.Companies;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace App.Services.CompanyServices
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<CreateCompanyResponse>> CreateAsync(CreateCompanyRequest request)
        {
            // Sadece Admin company oluşturabilir
            if (!IsAdmin())
            {
                return ServiceResult<CreateCompanyResponse>.Fail("Only Admin can create companies", HttpStatusCode.Forbidden);
            }

            var company = new Company()
            {
                Code = request.Code,
                Name = request.Name,
                TaxNumber = request.TaxNumber,
                Address = request.Address,
                Phone = request.Phone,
                UserId = request.UserId,
                IsActive = true,
            };

            await companyRepository.AddAsync(company);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateCompanyResponse>.Success(new CreateCompanyResponse(company.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCompanyRequest request)
        {
            // Editor sadece kendi company'sini güncelleyebilir, User hiçbirini güncelleyemez
            if (IsUser())
            {
                return ServiceResult.Fail("User role cannot update companies", HttpStatusCode.Forbidden);
            }

            var company = await companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return ServiceResult.Fail("Company not found", HttpStatusCode.NotFound);
            }

            // Editor sadece kendi company'sini güncelleyebilir
            if (IsEditor())
            {
                var userCompanyId = GetUserCompanyId();
                if (company.Id != userCompanyId)
                {
                    return ServiceResult.Fail("Editor can only update their own company", HttpStatusCode.Forbidden);
                }
            }

            company.Code = request.Code;
            company.Name = request.Name;
            company.Phone = request.Phone;
            company.IsActive = request.IsActive;
            company.TaxNumber = request.TaxNumber;
            company.Address = request.Address;
            

            companyRepository.Update(company);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);


        }

        public async Task<ServiceResult<List<CompanyDto>>> GetAllList()
        {
            var companies = await companyRepository.GetAllWithDetailsAsync();

            // Editor ve User sadece kendi company'lerini görebilir
            if (IsEditor() || IsUser())
            {
                var userCompanyId = GetUserCompanyId();
                companies = companies.Where(c => c.Id == userCompanyId).ToList();
            }

            var companyAsDto = companies.Select(c => new CompanyDto(
                c.Id,
                c.Code,
                c.Name,
                c.TaxNumber,
                c.Address,
                c.Phone,
                c.IsActive,
                c.User != null ? c.User.FullName : "N/A",
                c.Branches.Select(b => b.Name).ToList(),
                c.Warehouses.Select(w => w.Name).ToList()

            )).ToList();

            return ServiceResult<List<CompanyDto>>.Success(companyAsDto);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            // Company'yi navigation property'leri ile birlikte al
            var company = await companyRepository.Where(x => x.Id == id)
                .Include(x => x.Branches)
                .Include(x => x.StockCards)
                    .ThenInclude(sc => sc.BarcodeCards)
                .Include(x => x.Warehouses)
                .Include(x => x.BarcodeCards)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return ServiceResult.Fail("Company not found", HttpStatusCode.NotFound);
            }

            // Company soft delete
            company.IsActive = false;

            // Branch soft delete
            if (company.Branches != null && company.Branches.Any())
            {
                foreach (var branch in company.Branches)
                {
                    branch.IsActive = false;
                }
            }

            // Warehouse soft delete
            if (company.Warehouses != null && company.Warehouses.Any())
            {
                foreach (var warehouse in company.Warehouses)
                {
                    warehouse.IsActive = false;
                }
            }

            // StockCard ve BarcodeCard soft delete
            if (company.StockCards != null && company.StockCards.Any())
            {
                foreach (var stockCard in company.StockCards)
                {
                    stockCard.IsActive = false;

                    if (stockCard.BarcodeCards != null && stockCard.BarcodeCards.Any())
                    {
                        foreach (var barcode in stockCard.BarcodeCards)
                        {
                            barcode.IsActive = false;
                        }
                    }
                }
            }

            // Company BarcodeCards soft delete (direct ilişki)
            if (company.BarcodeCards != null && company.BarcodeCards.Any())
            {
                foreach (var barcode in company.BarcodeCards)
                {
                    barcode.IsActive = false;
                }
            }

            companyRepository.Update(company);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }




    }
}
