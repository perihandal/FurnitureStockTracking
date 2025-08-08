using App.Repositories;
using App.Repositories.SubGroups;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.Services.SubGroupServices
{
    public class SubGroupService : ISubGroupService
    {
        private readonly ISubGroupRepository subGroupRepository;
        private readonly IUnitOfWork unitOfWork;

        public SubGroupService(ISubGroupRepository subGroupRepository, IUnitOfWork unitOfWork)
        {
            this.subGroupRepository = subGroupRepository;
            this.unitOfWork = unitOfWork;
        }

        // Get all SubGroups
        public async Task<ServiceResult<List<SubGroupDto>>> GetAllListAsync()
        {
            var subGroups = await subGroupRepository.GetAllWithDetailsAsync();

            var subGroupsAsDto = subGroups.Select(sg => new SubGroupDto(
                sg.Id,                                   // Id
                sg.Code,                                 // Code
                sg.Name,                                 // Name
                sg.IsActive,                             // IsActive
                sg.UserId,                               // UserId
                sg.MainGroupId                    // MainGroupId
                //sg.StockCards.Select(sc => sc.Name).ToList() // StockCards isimlerini liste olarak al
            )).ToList();

            return ServiceResult<List<SubGroupDto>>.Success(subGroupsAsDto);
        }

        // Get SubGroup by Id
        public async Task<ServiceResult<SubGroupDto?>> GetByIdAsync(int id)
        {
            var subGroup = await subGroupRepository.GetByIdAsync(id);

            if (subGroup == null)
            {
                return ServiceResult<SubGroupDto?>.Fail("Sub group not found", HttpStatusCode.NotFound);
            }

            var subGroupAsDto = new SubGroupDto(
                 subGroup.Id,                // Id
                 subGroup.Code,              // Code
                 subGroup.Name,              // Name
                 subGroup.IsActive,          // IsActive
                 subGroup.UserId,            // UserId
                 subGroup.MainGroupId        // MainGroupId
 );

            return ServiceResult<SubGroupDto?>.Success(subGroupAsDto);
        }

        
        public async Task<ServiceResult<CreateSubGroupResponse>> CreateAsync(CreateSubGroupRequest request)
        {
            var subGroup = new SubGroup
            {
                Code = request.Code,
                Name = request.Name,
                UserId = request.UserId,
                MainGroupId = request.MainGroupId,
                IsActive = true, // default value
            };

            await subGroupRepository.AddAsync(subGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateSubGroupResponse>.Success(new CreateSubGroupResponse(subGroup.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateSubGroupRequest request)
        {
            var subGroup = await subGroupRepository.GetByIdAsync(id);

            if (subGroup == null)
            {
                return ServiceResult.Fail("Sub group not found", HttpStatusCode.NotFound);
            }

            subGroup.Code = request.Code;
            subGroup.Name = request.Name;
            subGroup.UserId = request.UserId;
            subGroup.MainGroupId = request.MainGroupId;
            subGroup.IsActive = request.IsActive;

            subGroupRepository.Update(subGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        // Delete a SubGroup
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var subGroup = await subGroupRepository.GetByIdAsync(id);

            if (subGroup == null)
            {
                return ServiceResult.Fail("Sub group not found", HttpStatusCode.NotFound);
            }

            subGroupRepository.Delete(subGroup);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
