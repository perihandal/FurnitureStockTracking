using App.Repositories.PriceHistories;
using App.Repositories;
using App.Services.PriceDefinitionServices;
using App.Services;
using System.Net;
using App.Repositories.StockCards;
using Microsoft.AspNetCore.Http;

public class PriceHistoryService : BaseService, IPriceHistoryService
{
    private readonly IPriceHistoryRepository pricehistoryRepository;
    private readonly IUnitOfWork unitOfWork;

    public PriceHistoryService(IPriceHistoryRepository pricehistoryRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        this.pricehistoryRepository = pricehistoryRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<List<PriceHistoryDto>>> GetAllListAsync()
    {
        var priceHistory = await pricehistoryRepository.GetAllWithDetailsAsync();

        // Editor ve User sadece kendi company ve branch'larındaki fiyat geçmişini görebilir
        if (IsEditor() || IsUser())
        {
            var userCompanyId = GetUserCompanyId();
            var userBranchId = GetUserBranchId();

            priceHistory = priceHistory.Where(ph => 
                ph.PriceDefinition.StockCard.CompanyId == userCompanyId && 
                ph.PriceDefinition.StockCard.BranchId == userBranchId).ToList();
        }

        var priceHistoriesAsDto = priceHistory.Select(ph => new PriceHistoryDto
        {
            StockCardId = ph.PriceDefinition.StockCard.Id,
            StockCardName = ph.PriceDefinition.StockCard.Name,
            OldPrice = ph.OldPrice,
            NewPrice = ph.NewPrice,
            ChangeDate = ph.ChangeDate,
        }).ToList();

        return ServiceResult<List<PriceHistoryDto>>.Success(priceHistoriesAsDto);
    }

    public async Task<ServiceResult<PriceHistoryDto?>> GetByIdAsync(int id)
    {
        var priceHistory = await pricehistoryRepository.GetAllWithDetailsAsync(id);

        if (priceHistory == null)
            return ServiceResult<PriceHistoryDto?>.Fail("Price history not found", HttpStatusCode.NotFound);

        // Editor ve User sadece kendi company ve branch'larındaki fiyat geçmişini görebilir
        if (IsEditor() || IsUser())
        {
            var accessCheck = ValidateEntityAccess(priceHistory.PriceDefinition.StockCard.CompanyId, priceHistory.PriceDefinition.StockCard.BranchId);
            if (!accessCheck.IsSuccess)
            {
                return ServiceResult<PriceHistoryDto?>.Fail("Access denied", HttpStatusCode.Forbidden);
            }
        }

        var priceHistoryAsDto = new PriceHistoryDto
        {
            StockCardId = priceHistory.PriceDefinition.StockCard.Id,
            StockCardName = priceHistory.PriceDefinition.StockCard.Name,
            OldPrice = priceHistory.OldPrice,
            NewPrice = priceHistory.NewPrice,
            ChangeDate = priceHistory.ChangeDate,
        };

        return ServiceResult<PriceHistoryDto?>.Success(priceHistoryAsDto);
    }
}
