using App.Repositories;
using App.Repositories.MainGroups;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Services.MainGroupServices
{
    public class MainGroupService : BaseService, IMainGroupService
    {
        private readonly IMainGroupRepository mainGroupRepository;
        private readonly IUnitOfWork unitOfWork;

        public MainGroupService(IMainGroupRepository mainGroupRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.mainGroupRepository = mainGroupRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<List<MainGroupDto>>> GetAllListAsync()
        {
            var mainGroups = await mainGroupRepository.GetAllWithDetailsAsync();

            // Admin tüm ana grupları görebilir
            // Editor ve User filtreleme yapmıyoruz çünkü MainGroup'ta CompanyId alanı yok
            // Gerekirse UserId ile filtreleme yapılabilir

            var mainGroupsAsDto = mainGroups.Select(mg => new MainGroupDto
            {
                Id = mg.Id,
                Code = mg.Code,
                Name = mg.Name,
                IsActive = mg.IsActive,
                UserId=mg.UserId,

                //SubGroups = mg.SubGroups.Select(sg => sg.Name).ToList(),
                //StockCards = mg.StockCards.Select(sc => sc.Name).ToList()
            }).ToList();

            return ServiceResult<List<MainGroupDto>>.Success(mainGroupsAsDto);
        }

        public async Task<ServiceResult<MainGroupDto?>> GetByIdAsync(int id)
        {
            var mainGroup = await mainGroupRepository.GetByIdAsync(id);

            if (mainGroup == null)
            {
                return ServiceResult<MainGroupDto?>.Fail("Main group not found", HttpStatusCode.NotFound);
            }

            var mainGroupAsDto = new MainGroupDto
            {
                Id = mainGroup.Id,
                Code = mainGroup.Code,
                Name = mainGroup.Name,
                IsActive = mainGroup.IsActive,
                UserId = mainGroup.UserId,
                //SubGroups = mainGroup.SubGroups.Select(sg => sg.Name).ToList(),
                //StockCards = mainGroup.StockCards.Select(sc => sc.Name).ToList()
            };

            return ServiceResult<MainGroupDto?>.Success(mainGroupAsDto);
        }


        public async Task<ServiceResult<CreateMainGroupResponse>> CreateAsync(CreateMainGroupRequest request)
        {
            // User yetkisi oluşturma işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult<CreateMainGroupResponse>.Fail("User role cannot create main groups", HttpStatusCode.Forbidden);
            }

            var mainGroup = new MainGroup
            {
                Code = request.Code,
                Name = request.Name,
                UserId = request.UserId,
                IsActive = true, // default value
            };

            await mainGroupRepository.AddAsync(mainGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateMainGroupResponse>.Success(new CreateMainGroupResponse(mainGroup.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateMainGroupRequest request)
        {
            // User yetkisi güncelleme işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("User role cannot update main groups", HttpStatusCode.Forbidden);
            }

            var mainGroup = await mainGroupRepository.GetByIdAsync(id);

            if (mainGroup == null)
            {
                return ServiceResult.Fail("Main group not found", HttpStatusCode.NotFound);
            }

            mainGroup.Code = request.Code;
            mainGroup.Name = request.Name;
            mainGroup.UserId = request.UserId;
            mainGroup.IsActive = request.IsActive;
            

            mainGroupRepository.Update(mainGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var mainGroup = await mainGroupRepository.GetByIdAsync(id);

            if (mainGroup == null)
            {
                return ServiceResult.Fail("Main group not found", HttpStatusCode.NotFound);
            }

            mainGroupRepository.Delete(mainGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
