using App.Repositories.PriceHistories;
using App.Repositories;
using App.Services.PriceDefinitionServices;
using App.Services;
using System.Net;

public class PriceHistoryService : IPriceHistoryService
{
    private readonly IPriceHistoryRepository pricehistoryRepository;
    private readonly IUnitOfWork unitOfWork;

    public PriceHistoryService(IPriceHistoryRepository pricehistoryRepository, IUnitOfWork unitOfWork)
    {
        this.pricehistoryRepository = pricehistoryRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<List<PriceHistoryDto>>> GetAllListAsync()
    {
        var priceHistory = await pricehistoryRepository.GetAllWithDetailsAsync();

        var priceHistoriesAsDto = priceHistory.Select(ph => new PriceHistoryDto
        {
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

        var priceHistoryAsDto = new PriceHistoryDto
        {
            StockCardName = priceHistory.PriceDefinition.StockCard.Name,
            OldPrice = priceHistory.OldPrice,
            NewPrice = priceHistory.NewPrice,
            ChangeDate = priceHistory.ChangeDate,
        };

        return ServiceResult<PriceHistoryDto?>.Success(priceHistoryAsDto);
    }
}
