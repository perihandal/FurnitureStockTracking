using App.Services.SubGroupServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class SubGroupController : CustomBaseController
    {
        private readonly ISubGroupService subGroupService;

       
        public SubGroupController(ISubGroupService subGroupService)
        {
            this.subGroupService = subGroupService;
        }

        // Tüm SubGroup'ları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await subGroupService.GetAllListAsync();
            return CreateActionResult(result);
        }

        // ID'ye göre SubGroup getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await subGroupService.GetByIdAsync(id));

        // Yeni SubGroup oluşturma
        [HttpPost]
        public async Task<IActionResult> Create(CreateSubGroupRequest request)
            => CreateActionResult(await subGroupService.CreateAsync(request));

        // Mevcut SubGroup güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSubGroupRequest request)
            => CreateActionResult(await subGroupService.UpdateAsync(id, request));

        // SubGroup silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => CreateActionResult(await subGroupService.DeleteAsync(id));
    }
}
