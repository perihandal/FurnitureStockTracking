using App.Services.MainGroupServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class MainGroupController : CustomBaseController
    {
        private readonly IMainGroupService mainGroupService;

        // Constructor Dependency Injection
        public MainGroupController(IMainGroupService mainGroupService)
        {
            this.mainGroupService = mainGroupService;
        }

        // Tüm MainGroup'ları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => CreateActionResult(await mainGroupService.GetAllListAsync());

        // ID'ye göre MainGroup getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await mainGroupService.GetByIdAsync(id));

        // Yeni MainGroup oluşturma
        [HttpPost]
        public async Task<IActionResult> Create(CreateMainGroupRequest request)
            => CreateActionResult(await mainGroupService.CreateAsync(request));

        // Mevcut MainGroup güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMainGroupRequest request)
            => CreateActionResult(await mainGroupService.UpdateAsync(id, request));

        // MainGroup silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => CreateActionResult(await mainGroupService.DeleteAsync(id));
    }
}
