//using App.Repositories.BarcodeCards;
//using App.Services.BarcodeCardServices;
//using Microsoft.AspNetCore.Mvc;

//namespace App.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BarcodeCardController : CustomBaseController
//    {
//        private readonly IBarcodeCardService _barcodeCardService;

//        public BarcodeCardController(IBarcodeCardService barcodeCardService)
//        {
//            _barcodeCardService = barcodeCardService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//            => CreateActionResult(await _barcodeCardService.GetAllListAsync());

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//            => CreateActionResult(await _barcodeCardService.GetByIdAsync(id));

//        [HttpGet("by-stockcard/{stockCardId}")]
//        public async Task<IActionResult> GetByStockCardId(int stockCardId)
//            => CreateActionResult(await _barcodeCardService.GetByStockCardIdAsync(stockCardId));

//        [HttpPost]
//        public async Task<IActionResult> Create(CreateBarcodeCardRequest request)
//            => CreateActionResult(await _barcodeCardService.CreateAsync(request));

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, UpdateBarcodeCardRequest request)
//            => CreateActionResult(await _barcodeCardService.UpdateAsync(id, request));

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//            => CreateActionResult(await _barcodeCardService.DeleteAsync(id));

//        [HttpPut("{id}/set-default")]
//        public async Task<IActionResult> SetAsDefault(int id)
//            => CreateActionResult(await _barcodeCardService.SetAsDefaultAsync(id));

//        [HttpGet("validate")]
//        public async Task<IActionResult> Validate([FromQuery] string barcodeCode, [FromQuery] BarcodeType barcodeType)
//            => CreateActionResult(await _barcodeCardService.ValidateBarcodeAsync(barcodeCode, barcodeType));
//    }
//}
