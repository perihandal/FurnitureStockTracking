using App.Services.CategoryServices;
using App.Services.WareHouseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class WarehouseController(IWareHouseService wareHouseService) : CustomBaseController
    {
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateWareHouseRequest request) => CreateActionResult(await wareHouseService.CreateAsync(request));

        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateWareHouseRequest request) => CreateActionResult(await wareHouseService.UpdateAsync(id, request));

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await wareHouseService.GetAllList());
    }
}
