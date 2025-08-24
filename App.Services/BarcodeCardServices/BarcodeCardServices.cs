using App.Repositories;
using App.Repositories.BarcodeCards;
using App.Repositories.StockCards;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using App.Services.BarcodeCardGeneratorService;
using App.Services.BarcodeCardValidationService;

namespace App.Services.BarcodeCardServices
{
    public class BarcodeCardService : IBarcodeCardService
    {
        private readonly IBarcodeCardRepository barcodeCardRepository;
        private readonly IStockCardRepository stockCardRepository;
        private readonly IBarcodeGeneratorService barcodeGeneratorService;
        private readonly IBarcodeValidationService barcodeValidationService;
        private readonly IUnitOfWork unitOfWork;

        public BarcodeCardService(
            IBarcodeCardRepository barcodeCardRepository,
            IStockCardRepository stockCardRepository,
            IBarcodeGeneratorService barcodeGeneratorService,
            IBarcodeValidationService barcodeValidationService,
            IUnitOfWork unitOfWork)
        {
            this.barcodeCardRepository = barcodeCardRepository;
            this.stockCardRepository = stockCardRepository;
            this.barcodeGeneratorService = barcodeGeneratorService;
            this.barcodeValidationService = barcodeValidationService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<List<BarcodeCardDto>>> GetAllListAsync()
        {
            var barcodeCards = await barcodeCardRepository.GetAllWithDetailsAsync();

            var barcodeCardsAsDto = barcodeCards.Select(bc => new BarcodeCardDto
            {
                Id = bc.Id,
                BarcodeCode = bc.BarcodeCode,
                BarcodeType = bc.BarcodeType,
                IsDefault = bc.IsDefault,
                StockCardId = bc.StockCardId,
                StockCardName = bc.StockCard?.Name,
                UserId = bc.UserId,
                CreateDate = bc.CreateDate,
                BranchId = bc.BranchId,
                CompanyId = bc.CompanyId
            }).ToList();

            return ServiceResult<List<BarcodeCardDto>>.Success(barcodeCardsAsDto);
        }

        public async Task<ServiceResult<BarcodeCardDto?>> GetByIdAsync(int id)
        {
            var barcodeCard = await barcodeCardRepository.GetByIdAsync(id);

            if (barcodeCard == null)
            {
                return ServiceResult<BarcodeCardDto?>.Fail("Barcode card not found", HttpStatusCode.NotFound);
            }

            var barcodeCardAsDto = new BarcodeCardDto
            {
                Id = barcodeCard.Id,
                BarcodeCode = barcodeCard.BarcodeCode,
                BarcodeType = barcodeCard.BarcodeType,
                IsDefault = barcodeCard.IsDefault,
                StockCardId = barcodeCard.StockCardId,
                StockCardName = barcodeCard.StockCard?.Name,
                UserId = barcodeCard.UserId,
                CreateDate = barcodeCard.CreateDate,
                BranchId = barcodeCard.BranchId,
                CompanyId = barcodeCard.CompanyId
            };

            return ServiceResult<BarcodeCardDto?>.Success(barcodeCardAsDto);
        }

        public async Task<ServiceResult<List<BarcodeCardDto>>> GetByStockCardIdAsync(int stockCardId)
        {
            var barcodeCards = await barcodeCardRepository.GetByStockCardIdAsync(stockCardId);

            var barcodeCardsAsDto = barcodeCards.Select(bc => new BarcodeCardDto
            {
                Id = bc.Id,
                BarcodeCode = bc.BarcodeCode,
                BarcodeType = bc.BarcodeType,
                IsDefault = bc.IsDefault,
                StockCardId = bc.StockCardId,
                StockCardName = bc.StockCard?.Name,
                UserId = bc.UserId,
                CreateDate = bc.CreateDate,
                BranchId = bc.BranchId,
                CompanyId = bc.CompanyId
            }).ToList();

            return ServiceResult<List<BarcodeCardDto>>.Success(barcodeCardsAsDto);
        }

        public async Task<ServiceResult<CreateBarcodeCardResponse>> CreateAsync(CreateBarcodeCardRequest request)
        {
            // Stok kartının var olup olmadığını kontrol et
            var stockCard = await stockCardRepository.GetByIdAsync(request.StockCardId);
            if (stockCard == null)
            {
                return ServiceResult<CreateBarcodeCardResponse>.Fail("Stock card not found", HttpStatusCode.NotFound);
            }

            // Eğer bu varsayılan barkod olarak işaretleniyorsa, diğerlerini varsayılan olmaktan çıkar
            if (request.IsDefault)
            {
                await SetOtherBarcodesAsNonDefaultAsync(request.StockCardId);
            }

            // Barkod kodunu üret
            var barcodeCode = barcodeGeneratorService.GenerateUniqueBarcode(
                request.BarcodeType,
                request.StockCardId,
                request.CompanyId,
                code => barcodeCardRepository.ExistsByBarcodeCodeAsync(code).Result
            );

            var barcodeCard = new BarcodeCard
            {
                BarcodeCode = barcodeCode,
                BarcodeType = request.BarcodeType,
                IsDefault = request.IsDefault,
                StockCardId = request.StockCardId,
                UserId = request.UserId,
                BranchId = request.BranchId,
                CompanyId = request.CompanyId,
                CreateDate = DateTime.UtcNow
            };

            await barcodeCardRepository.AddAsync(barcodeCard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateBarcodeCardResponse>.Success(new CreateBarcodeCardResponse(barcodeCard.Id, barcodeCode));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateBarcodeCardRequest request)
        {
            var barcodeCard = await barcodeCardRepository.GetByIdAsync(id);

            if (barcodeCard == null)
            {
                return ServiceResult.Fail("Barcode card not found", HttpStatusCode.NotFound);
            }

            // Eğer bu varsayılan barkod olarak işaretleniyorsa, diğerlerini varsayılan olmaktan çıkar
            if (request.IsDefault && !barcodeCard.IsDefault)
            {
                await SetOtherBarcodesAsNonDefaultAsync(barcodeCard.StockCardId, id);
            }

            barcodeCard.IsDefault = request.IsDefault;
            barcodeCard.UserId = request.UserId;
            barcodeCard.BranchId = request.BranchId;

            barcodeCardRepository.Update(barcodeCard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var barcodeCard = await barcodeCardRepository.GetByIdAsync(id);

            if (barcodeCard == null)
            {
                return ServiceResult.Fail("Barcode card not found", HttpStatusCode.NotFound);
            }

            barcodeCardRepository.Delete(barcodeCard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> SetAsDefaultAsync(int id)
        {
            var barcodeCard = await barcodeCardRepository.GetByIdAsync(id);

            if (barcodeCard == null)
            {
                return ServiceResult.Fail("Barcode card not found", HttpStatusCode.NotFound);
            }

            // Diğer barkodları varsayılan olmaktan çıkar
            await SetOtherBarcodesAsNonDefaultAsync(barcodeCard.StockCardId, id);

            barcodeCard.IsDefault = true;
            barcodeCardRepository.Update(barcodeCard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> ValidateBarcodeAsync(string barcodeCode, BarcodeType barcodeType)
        {
            var isValid = barcodeValidationService.ValidateBarcode(barcodeCode, barcodeType);

            if (!isValid)
            {
                return ServiceResult.Fail($"Invalid barcode format for type {barcodeType}", HttpStatusCode.BadRequest);
            }

            return ServiceResult.Success(HttpStatusCode.OK);
        }

        private async Task SetOtherBarcodesAsNonDefaultAsync(int stockCardId, int? excludeId = null)
        {
            var otherBarcodes = await barcodeCardRepository.GetByStockCardIdAsync(stockCardId);

            foreach (var barcode in otherBarcodes.Where(b => b.IsDefault && b.Id != excludeId))
            {
                barcode.IsDefault = false;
                barcodeCardRepository.Update(barcode);
            }
        }
    }
}