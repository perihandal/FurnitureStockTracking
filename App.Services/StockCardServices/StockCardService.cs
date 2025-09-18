using Microsoft.EntityFrameworkCore;
using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.StockCards;
using App.Repositories.BarcodeCards;
using App.Services.BarcodeCardGeneratorService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Services.StockCardServices
{
    public class StockCardService : BaseService, IStockCardService
    {
        private readonly IStockCardRepository stockcardRepository;
        private readonly IBarcodeCardRepository barcodeCardRepository;
        private readonly IBarcodeGeneratorService barcodeGeneratorService;
        private readonly IUnitOfWork unitOfWork;

        public StockCardService(
            IStockCardRepository stockcardRepository,
            IBarcodeCardRepository barcodeCardRepository,
            IBarcodeGeneratorService barcodeGeneratorService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.stockcardRepository = stockcardRepository;
            this.barcodeCardRepository = barcodeCardRepository;
            this.barcodeGeneratorService = barcodeGeneratorService;
            this.unitOfWork = unitOfWork;
        }
        //public async Task<ServiceResult<List<StockCardDto>>> GetTopPriceASync(int count)
        //{
        //    var stockcards = await stockcardRepository.GetTopPriceProductsAsync(count);

        //    var productsAsDto = stockcards.Select(p => new StockCardDto(p.Id, p.Name)).ToList();

        //    return new ServiceResult<List<StockCardDto>>()
        //    {
        //        Data = productsAsDto
        //    };
        //}
        //public async Task<ServiceResult<StockCardDto?>> GetByIdAsync(int id)
        //{
        //    var stockcard = await stockcardRepository.GetByIdAsync(id);

        //    if (stockcard == null)
        //    {
        //        ServiceResult<StockCardDto>.Fail(errorMessage: "Product not found", HttpStatusCode.NotFound);
        //    }

        //    var productsAsDto = new StockCardDto(stockcard!.Id, stockcard.Name, stockcard.Price);

        //    return ServiceResult<StockCardDto>.Success((StockCardDto)productsAsDto)!;

        //}

        public async Task<ServiceResult<CreateStockCardResponse>> CreateAsync(CreateStockCardRequest request)
        {
            // User rolü create işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult<CreateStockCardResponse>.Fail("Stok kartı oluşturma yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // CompanyId ve BranchId doğrulaması
            var accessValidation = ValidateEntityAccess(request.CompanyId, request.BranchId);
            if (!accessValidation.IsSuccess)
            {
                return ServiceResult<CreateStockCardResponse>.Fail(accessValidation.ErrorMessage!, accessValidation.Status);
            }

            var anyStockCard = await stockcardRepository
                .Where(x => x.CompanyId == request.CompanyId && x.Name == request.Name)
                .AnyAsync();
            if (anyStockCard)
            {
                return ServiceResult<CreateStockCardResponse>.Fail("Bu isimde bir ürün zaten mevcut.", HttpStatusCode.BadRequest);
            }

            var anyStockCardWithCode = await stockcardRepository
                .Where(x => x.CompanyId == request.CompanyId && x.Code == request.Code)
                .AnyAsync();

            if (anyStockCardWithCode)
            {
                return ServiceResult<CreateStockCardResponse>.Fail(
                    "Bu stok kodu zaten mevcut.",
                    HttpStatusCode.BadRequest
                );
            }
            var stockcard = new StockCard()
            {
                Name = request.Name,
                Code = request.Code,
                Tax = request.Tax,
                Type = request.Type,
                Unit = request.Unit,
                SubGroupId = request.SubGroupId,
                CompanyId = request.CompanyId,
                MainGroupId = request.MainGroupId,
                BranchId = request.BranchId,
                CategoryId = request.CategoryId,
                UserId = request.UserId,

                // default değerler:
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await stockcardRepository.AddAsync(stockcard);
            await unitOfWork.SaveChangesAsync();

            string? defaultBarcodeCode = null;

            // Eğer varsayılan barkod oluşturulması isteniyorsa
            if (request.CreateDefaultBarcode)
            {
                try
                {
                    // Benzersiz barkod üret
                    defaultBarcodeCode = barcodeGeneratorService.GenerateUniqueBarcode(
                        request.DefaultBarcodeType,
                        stockcard.Id,
                        request.CompanyId,
                        code => barcodeCardRepository.ExistsByBarcodeCodeAsync(code).Result
                    );

                    var barcodeCard = new BarcodeCard
                    {
                        BarcodeCode = defaultBarcodeCode,
                        BarcodeType = request.DefaultBarcodeType,
                        IsDefault = true,
                        StockCardId = stockcard.Id,
                        UserId = request.UserId,
                        BranchId = request.BranchId,
                        CompanyId = request.CompanyId,
                        CreateDate = DateTime.UtcNow
                    };

                    await barcodeCardRepository.AddAsync(barcodeCard);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Barkod oluşturulamazsa warning olarak devam et, stock card'ı geri alma
                    // Log the error if logging is available
                    defaultBarcodeCode = null;
                }
            }

            return ServiceResult<CreateStockCardResponse>.Success(new CreateStockCardResponse(stockcard.Id, defaultBarcodeCode));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateStockCardRequest request)
        {
            // User yetkisi güncelleme işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("User role cannot update stock cards", HttpStatusCode.Forbidden);
            }

            var stockcard = await stockcardRepository.GetByIdAsync(id);

            if (stockcard == null)
            {
                return ServiceResult.Fail("StockCard not found", HttpStatusCode.NotFound);
            }

            // Editor sadece kendi company ve branch'ındaki kartları güncelleyebilir
            if (IsEditor())
            {
                var accessCheck = ValidateEntityAccess(stockcard.CompanyId, stockcard.BranchId);
                if (!accessCheck.IsSuccess)
                {
                    return accessCheck;
                }
            }

            stockcard.Name = request.Name;
            stockcard.Code = request.Code;
            stockcard.Tax = request.Tax;
            stockcard.Type = request.Type;
            stockcard.Unit = request.Unit;
            stockcard.SubGroupId = request.SubGroupId;
            stockcard.CompanyId = request.CompanyId;
            stockcard.MainGroupId = request.MainGroupId;
            stockcard.BranchId = request.BranchId;
            stockcard.CategoryId = request.CategoryId;
            stockcard.UserId = request.UserId;
            stockcard.IsActive = request.IsActive;

            stockcardRepository.Update(stockcard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);

        }


        public async Task<ServiceResult<List<StockCardDto>>> GetAllList()
        {
            var stockcards = await stockcardRepository.GetAllWithDetailsAsync();

            // Editor ve User sadece kendi company/branch'larındaki kartları görebilir
            if (IsEditor() || IsUser())
            {
                var userCompanyId = GetUserCompanyId();
                var userBranchId = GetUserBranchId();

                stockcards = stockcards.Where(s => s.CompanyId == userCompanyId && s.BranchId == userBranchId).ToList();
            }

            var stockcardsAsDto = stockcards.Select(sc => new StockCardDto(
                sc.Id,
                sc.Name,
                sc.Code,
                sc.Type,
                sc.Unit,
                sc.Tax,
                sc.CreatedDate,
                sc.Company.Id,
                sc.Company?.Name ?? "Unknown Company",
                sc.Branch.Id,
                sc.Branch?.Name ?? "Unknown Branch",
                sc.MainGroup.Id,
                sc.MainGroup?.Name ?? "Unknown MainGroup",
                sc.SubGroup?.Id ?? 0,
                sc.SubGroup?.Name ?? "Unknown SubGroup",
                sc.Category?.Id ?? 0,
                sc.Category?.Name ?? "Unknown Category",
                sc.BarcodeCards?.Select(bc => bc.BarcodeCode).ToList() ?? new List<string>()
            )).ToList();

            return ServiceResult<List<StockCardDto>>.Success(stockcardsAsDto);
        }

        public async Task<ServiceResult<List<StockCardDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var baseQuery = stockcardRepository.GetAll()
                .Where(s => s.IsActive); // sadece aktif olanlar

            // Editor ve User sadece kendi company/branch'larındaki kartları görebilir
            if (IsEditor() || IsUser())
            {
                var userCompanyId = GetUserCompanyId();
                var userBranchId = GetUserBranchId();

                baseQuery = baseQuery.Where(s => s.CompanyId == userCompanyId && s.BranchId == userBranchId);
            }

            var stockCardsQuery = baseQuery
                .Include(s => s.Company)
                .Include(s => s.Branch)
                .Include(s => s.MainGroup)
                .Include(s => s.SubGroup)
                .Include(s => s.Category)
                .Include(s => s.BarcodeCards);

            var stockCards = await stockCardsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var stockCardAsDto = stockCards.Select(p => new StockCardDto(
                p.Id,
                p.Name,
                p.Code,
                p.Type,
                p.Unit,
                p.Tax,
                p.CreatedDate,
                p.Company.Id,
                p.Company?.Name ?? "N/A",
                p.Branch.Id,
                p.Branch?.Name ?? "N/A",
                p.MainGroup.Id, 
                p.MainGroup?.Name ?? "N/A",
                p.SubGroup?.Id ?? 0,
                p.SubGroup?.Name ?? "N/A",
                p.Category?.Id ?? 0,
                p.Category?.Name ?? "N/A",
                p.BarcodeCards?.Select(b => b.BarcodeCode).ToList() ?? new List<string>()
            )).ToList();

            return ServiceResult<List<StockCardDto>>.Success(stockCardAsDto);
        }


        public async Task<ServiceResult> DeleteAsync(int id)
        {
            // User yetkisi silme işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("User role cannot delete stock cards", HttpStatusCode.Forbidden);
            }

            // StockCard'ı navigation property'leri ile birlikte al
            var stockCard = await stockcardRepository.Where(x => x.Id == id)
                .Include(x => x.BarcodeCards)
                .Include(x => x.PriceDefinitions)
                .ThenInclude(pd => pd.PriceHistories) // Eğer PriceHistories navigation property varsa
                .FirstOrDefaultAsync();

            if (stockCard == null)
            {
                return ServiceResult.Fail("StockCard not found", HttpStatusCode.NotFound);
            }

            // Editor sadece kendi company ve branch'ındaki kartları silebilir
            if (IsEditor())
            {
                var accessCheck = ValidateEntityAccess(stockCard.CompanyId, stockCard.BranchId);
                if (!accessCheck.IsSuccess)
                {
                    return accessCheck;
                }
            }

            // StockCard soft delete
            stockCard.IsActive = false;

            //Barcode kartları soft delete
            if (stockCard.BarcodeCards != null && stockCard.BarcodeCards.Any())
            {
                foreach (var barcode in stockCard.BarcodeCards)
                {
                    barcode.IsActive = false; 
                }
            }

            // PriceDefinition ve PriceHistory soft delete
            if (stockCard.PriceDefinitions != null && stockCard.PriceDefinitions.Any())
            {
                foreach (var pd in stockCard.PriceDefinitions)
                {
                    pd.IsActive = false; // PriceDefinition tablosuna IsActive eklemeyi unutma

                    if (pd.PriceHistories != null && pd.PriceHistories.Any())
                    {
                        foreach (var ph in pd.PriceHistories)
                        {
                            ph.IsActive = false; // PriceHistory tablosuna IsActive eklemeyi unutma
                        }
                    }
                }
            }
            stockcardRepository.Update(stockCard);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

    }
}