using App.Services.CategoryServices;
using App.Services.CompanyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.API.Auth;

namespace App.API.Controllers
{
    [Authorize(Roles = "Admin,Editor,User")]
    public class CompanyController(ICompanyService companyService) : CustomBaseController
    {
        [RoleAuthorize("Admin")] // Sadece Admin şirket oluşturabilir
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyRequest request) => CreateActionResult(await companyService.CreateAsync(request));

        [RoleAuthorize("Admin")] // Sadece Admin şirket güncelleyebilir
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCompanyRequest request) => CreateActionResult(await companyService.UpdateAsync(id, request));

        [CompanyAuthorize] // Şirket bazlı filtreleme
        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResult(await companyService.GetAllList());

        [RoleAuthorize("Admin")] // Sadece Admin şirket silebilir
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await companyService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}
