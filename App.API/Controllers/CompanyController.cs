using App.Services.CategoryServices;
using App.Services.CompanyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    [Authorize]
    public class CompanyController(ICompanyService companyService) : CustomBaseController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateCompanyRequest request) => CreateActionResult(await companyService.CreateAsync(request));

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCompanyRequest request) => CreateActionResult(await companyService.UpdateAsync(id, request));


        [Authorize(Roles = "Admin,Editor")]
        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await companyService.GetAllList());

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await companyService.DeleteAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("by-user/{userId}")] // User'ın companyId'sini getir
        public async Task<IActionResult> GetCompanyIdByUserId(int userId)
        {
            var result = await companyService.GetCompanyIdByUserIdAsync(userId);

            // ServiceResult tipi zaten Success/Fail durumunu içeriyor
            return CreateActionResult(result);
        }
    }
}
