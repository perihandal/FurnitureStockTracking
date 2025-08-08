using App.Repositories.Branches;
using App.Repositories.Companies;
using App.Repositories.Warehouses;
using System.Net;
using App.Services.CompanyServices;
using App.Repositories.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Repositories;
using App.Services.StockCardServices;

namespace App.Services.BranchServices
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
        {
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<CreateBranchResponse>> CreateAsync(CreateBranchRequest request)
        {
            var branch = new Branch()
            {
                Code = request.Code,              // Şube kodu
                Name = request.Name,              // Şube adı
                Address = request.Address,        // Adres
                Phone = request.Phone,            // Telefon
                IsActive = request.IsActive,      // Eğer `IsActive` request'ten geliyorsa, yoksa default olarak `true` olabilir
                CompanyId = request.CompanyId,
                UserId = request.UserId, 
            };

            await _branchRepository.AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateBranchResponse>.Success(new CreateBranchResponse(branch.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateBranchRequest request)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                return ServiceResult.Fail("Branch not found", HttpStatusCode.NotFound);
            }

            branch.Code = request.Code;
            branch.Name = request.Name;
            branch.Address = request.Address;
            branch.Phone = request.Phone;
            branch.IsActive = request.IsActive;
            branch.CompanyId = request.CompanyId;
            branch.UserId = request.UserId;

            _branchRepository.Update(branch);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<BranchDto>>> GetAllList()
        {
            var branches = await _branchRepository.GetAllWithDetailsAsync();

            var branchDtos = branches.Select(b => new BranchDto(
                b.Id,                             // ID'yi ekledim
                b.Code,
                b.Name,
                b.Address,
                b.Phone,
                b.IsActive,
                b.Company.Name,
                b.User.FullName,
                b.Warehouses.Select(w => w.Name).ToList(), // Depo isimlerini liste olarak alıyorum
                b.StockCards.Select(s => s.Code).ToList()  // Stok kartlarını liste olarak alıyorum
            )).ToList();

            return ServiceResult<List<BranchDto>>.Success(branchDtos);
        }

        //public async Task<ServiceResult<BranchDto>> GetByIdAsync(int id)
        //{
        //    var branch = await _branchRepository.GetByIdAsync(id);

        //    if (branch == null)
        //    {
        //        return ServiceResult<BranchDto>.Fail("Branch not found", HttpStatusCode.NotFound);
        //    }

        //    var branchDto = new BranchDto(
        //        branch.Id,                        // ID'yi ekledim
        //        branch.Code,
        //        branch.Name,
        //        branch.Address,
        //        branch.Phone,
        //        branch.IsActive,
        //        branch.Company.Name,
        //        branch.User.FullName,
        //        branch.Warehouses.Select(w => w.Name).ToList(), // Depo isimlerini liste olarak alıyorum
        //        branch.StockCards.Select(s => s.Code).ToList()  // Stok kartlarını liste olarak alıyorum
        //    );

        //    return ServiceResult<BranchDto>.Success((BranchDto)branchDto);
        //}

        public async Task<ServiceResult<BranchDto?>> GetByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                ServiceResult<StockCardDto>.Fail(errorMessage: "Product not found", HttpStatusCode.NotFound);
            }

            var branchAsDto = new BranchDto(
                branch.Id,                        // ID'yi ekledim
                branch.Code,
                branch.Name,
                branch.Address,
                branch.Phone,
                branch.IsActive,
                branch.Company.Name,
                branch.User.FullName,
                branch.Warehouses.Select(w => w.Name).ToList(), // Depo isimlerini liste olarak alıyorum
                branch.StockCards.Select(s => s.Code).ToList()  // Stok kartlarını liste olarak alıyorum
             );

            return ServiceResult<BranchDto>.Success((BranchDto)branchAsDto)!;

        }

    }
}
