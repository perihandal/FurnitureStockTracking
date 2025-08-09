using App.Repositories;
using App.Repositories.PriceDefinitions;
using App.Repositories.StockCards;
using App.Repositories.PriceHistories; // PriceHistory ekleme için
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.Services.PriceDefinitionServices
{
    public class PriceDefinitionService : IPriceDefinitionService
    {
        private readonly IPriceDefinitionRepository _priceDefinitionRepository;
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PriceDefinitionService(
            IPriceDefinitionRepository priceDefinitionRepository,
            IPriceHistoryRepository priceHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _priceDefinitionRepository = priceDefinitionRepository;
            _priceHistoryRepository = priceHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<List<PriceDefinitionDto>>> GetAllListAsync()
        {
            var priceDefinitions = await _priceDefinitionRepository.GetAllWithDetailsAsync();

            var priceDefinitionsAsDto = priceDefinitions.Select(pd => new PriceDefinitionDto
            {
                Id = pd.Id,
                PriceType = pd.PriceType,
                Price = pd.Price,
                Currency = pd.Currency,
                ValidFrom = pd.ValidFrom,
                ValidTo = pd.ValidTo,
                IsActive = pd.ValidTo == null || pd.ValidTo > DateTime.UtcNow,
                StockCardName = pd.StockCard.Name,
                UserFullName = pd.User.FullName
            }).ToList();

            return ServiceResult<List<PriceDefinitionDto>>.Success(priceDefinitionsAsDto);
        }

        public async Task<ServiceResult<PriceDefinitionDto?>> GetByIdAsync(int id)
        {
            var priceDefinition = await _priceDefinitionRepository.GetAllWithDetailsAsync(id);

            if (priceDefinition == null)
            {
                return ServiceResult<PriceDefinitionDto?>.Fail("Price definition not found", HttpStatusCode.NotFound);
            }

            var priceDefinitionAsDto = new PriceDefinitionDto
            {
                Id = priceDefinition.Id,
                PriceType = priceDefinition.PriceType,
                Price = priceDefinition.Price,
                Currency = priceDefinition.Currency,
                ValidFrom = priceDefinition.ValidFrom,
                ValidTo = priceDefinition.ValidTo,
                IsActive = priceDefinition.ValidTo == null || priceDefinition.ValidTo > DateTime.UtcNow,
                StockCardName = priceDefinition.StockCard.Name,
                UserFullName = priceDefinition.User.FullName
            };

            return ServiceResult<PriceDefinitionDto?>.Success(priceDefinitionAsDto);
        }

        public async Task<ServiceResult<CreatePriceDefinitionResponse>> CreateAsync(CreatePriceDefinitionRequest request)
        {
            var priceDefinition = new PriceDefinition
            {
                PriceType = request.PriceType,
                Price = request.Price,
                Currency = request.Currency,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                UserId = request.UserId,
                StockCardId = request.StockCardId
            };

            priceDefinition.IsActive = priceDefinition.ValidTo == null || priceDefinition.ValidTo > DateTime.UtcNow;

            await _priceDefinitionRepository.AddAsync(priceDefinition);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CreatePriceDefinitionResponse>.Success(new CreatePriceDefinitionResponse(priceDefinition.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdatePriceDefinitionRequest request)
        {
            var priceDefinition = await _priceDefinitionRepository.GetByIdAsync(id);

            if (priceDefinition == null)
            {
                return ServiceResult.Fail("Price definition not found", HttpStatusCode.NotFound);
            }

            // Eski fiyatı sakla
            var oldPrice = priceDefinition.Price;

            // Fiyat değişmişse PriceHistory kaydı oluştur
            if (oldPrice != request.Price)
            {
                var history = new PriceHistory
                {
                    PriceDefinitionId = priceDefinition.Id,
                    PriceType = priceDefinition.PriceType,
                    OldPrice = oldPrice,
                    NewPrice = request.Price,
                    ChangeDate = DateTime.UtcNow
                };

                await _priceHistoryRepository.AddAsync(history);
            }

            // PriceDefinition güncelle
            priceDefinition.PriceType = request.PriceType;
            priceDefinition.Price = request.Price;
            priceDefinition.Currency = request.Currency;
            priceDefinition.ValidFrom = request.ValidFrom;
            priceDefinition.ValidTo = request.ValidTo;
            priceDefinition.UserId = request.UserId;
            priceDefinition.IsActive = priceDefinition.ValidTo == null || priceDefinition.ValidTo > DateTime.UtcNow;

            _priceDefinitionRepository.Update(priceDefinition);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var priceDefinition = await _priceDefinitionRepository.GetByIdAsync(id);

            if (priceDefinition == null)
            {
                return ServiceResult.Fail("Price definition not found", HttpStatusCode.NotFound);
            }

            priceDefinition.IsActive = false;

            _priceDefinitionRepository.Delete(priceDefinition);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
