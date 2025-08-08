using App.Services.CategoryServices;
using App.Services.CompanyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    public class CompanyController(ICompanyService companyService) : CustomBaseController
    {
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateCompanyRequest request) => CreateActionResult(await companyService.CreateAsync(request));

        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCompanyRequest request) => CreateActionResult(await companyService.UpdateAsync(id, request));

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await companyService.GetAllList());
    }
}
