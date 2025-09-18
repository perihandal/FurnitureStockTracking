using App.Services.BranchServices;
using App.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    [Authorize]
    public class BranchController(IBranchService _branchService) : CustomBaseController
    {
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost] // Yeni şube oluşturmak için
        public async Task<IActionResult> Create(CreateBranchRequest request)
            => CreateActionResult(await _branchService.CreateAsync(request));

        [Authorize(Roles = "Admin,Editor")]
        [HttpPut] // Şube güncelleme
        public async Task<IActionResult> Update(int id, UpdateBranchRequest request)
            => CreateActionResult(await _branchService.UpdateAsync(id, request));

        [Authorize(Roles = "Admin,Editor")]
        [HttpGet] // Tüm şubeleri listelemek için
        public async Task<IActionResult> GetAll()
            => CreateActionResult(await _branchService.GetAllList());

        [Authorize(Roles = "Admin,Editor")]
        [HttpGet("{id}")] // Belirli bir şubeyi id ile almak için
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await _branchService.GetByIdAsync(id));

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}
