using App.Services.BranchServices;
using App.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
  
    public class BranchController(IBranchService _branchService) : CustomBaseController
    {
        [HttpPost] // Yeni şube oluşturmak için
        public async Task<IActionResult> Create(CreateBranchRequest request)
            => CreateActionResult(await _branchService.CreateAsync(request));

        [HttpPut] // Şube güncelleme
        public async Task<IActionResult> Update(int id, UpdateBranchRequest request)
            => CreateActionResult(await _branchService.UpdateAsync(id, request));

        [HttpGet] // Tüm şubeleri listelemek için
        public async Task<IActionResult> GetAll()
            => CreateActionResult(await _branchService.GetAllList());

        [HttpGet("{id}")] // Belirli bir şubeyi id ile almak için
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await _branchService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}
