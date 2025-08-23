using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Repositories.StockTransactions;
using App.Repositories;
using App.Repositories.WarehouseStocks;
using App.Services;
using App.Services.StockTransactionServices;
public class StockTransactionService : IStockTransactionService
{
    private readonly IStockTransactionRepository _stockTransactionRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StockTransactionService(
        IStockTransactionRepository stockTransactionRepository,
        IWarehouseStockRepository warehouseStockRepository,
        IUnitOfWork unitOfWork)
    {
        _stockTransactionRepository = stockTransactionRepository;
        _warehouseStockRepository = warehouseStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<List<StockTransactionDto>>> GetAllAsync()
    {
        var transactions = await _stockTransactionRepository.GetAllWithDetailsAsync();
        var dtos = transactions.Select(MapToDto).ToList();
        return ServiceResult<List<StockTransactionDto>>.Success(dtos);
    }

    public async Task<ServiceResult<StockTransactionDto?>> GetByIdAsync(int id)
    {
        var transaction = await _stockTransactionRepository.GetByIdWithDetailsAsync(id);
        if (transaction == null)
            return ServiceResult<StockTransactionDto?>.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        var dto = MapToDto(transaction);
        return ServiceResult<StockTransactionDto?>.Success(dto);
    }

    public async Task<ServiceResult<CreateStockTransactionResponse>> CreateAsync(CreateStockTransactionRequest request)
    {
        // 1. Validation
        var validationResult = ValidateTransactionRequest(request);
        if (!validationResult.IsSuccess)
            return ServiceResult<CreateStockTransactionResponse>.Fail(validationResult.ErrorMessage);

        // 2. Transaction entity oluşturma
        var entity = CreateTransactionEntity(request);

        // 3. Warehouse stock işlemleri
        var stockResult = await ProcessWarehouseStocks(entity, isCreate: true);
        if (!stockResult.IsSuccess)
            return ServiceResult<CreateStockTransactionResponse>.Fail(stockResult.ErrorMessage);

        // 4. Entity'i kaydet
        await _stockTransactionRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<CreateStockTransactionResponse>.Success(new CreateStockTransactionResponse(entity.Id));
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateStockTransactionRequest request)
    {
        // 1. Mevcut transaction'ı bul
        var existingEntity = await _stockTransactionRepository.GetByIdAsync(id);
        if (existingEntity == null)
            return ServiceResult.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        // 2. Validation
        var validationResult = ValidateTransactionRequest(request);
        if (!validationResult.IsSuccess)
            return ServiceResult.Fail(validationResult.ErrorMessage);

        // 3. Eski transaction'ı geri al (reverse operation)
        var reverseResult = await ReverseTransactionStocks(existingEntity);
        if (!reverseResult.IsSuccess)
            return ServiceResult.Fail(reverseResult.ErrorMessage);

        // 4. Entity'i güncelle
        UpdateEntityFromRequest(existingEntity, request);

        // 5. Yeni transaction'ı uygula
        var stockResult = await ProcessWarehouseStocks(existingEntity, isCreate: false);
        if (!stockResult.IsSuccess)
        {
            // Hata durumunda rollback yap
            await ReverseTransactionStocks(existingEntity, isRollback: true);
            return ServiceResult.Fail(stockResult.ErrorMessage);
        }

        _stockTransactionRepository.Update(existingEntity);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var entity = await _stockTransactionRepository.GetByIdAsync(id);
        if (entity == null)
            return ServiceResult.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        // 1. Transaction'ı geri al
        var reverseResult = await ReverseTransactionStocks(entity);
        if (!reverseResult.IsSuccess)
            return ServiceResult.Fail($"Stok işlemi geri alınamadı: {reverseResult.ErrorMessage}");

        // 2. Entity'i sil
        _stockTransactionRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    #region Private Helper Methods

    private StockTransaction CreateTransactionEntity(CreateStockTransactionRequest request)
    {
        var entity = new StockTransaction
        {
            Type = request.Type,
            Quantity = request.Quantity,
            TransactionDate = request.TransactionDate,
            DocumentNumber = request.DocumentNumber,
            Description = request.Description,
            StockCardId = request.StockCardId,
            FromWarehouseId = request.FromWarehouseId,
            ToWarehouseId = request.ToWarehouseId,
            UserId = request.UserId
        };

        // Transfer işleminde WarehouseId null olmalı
        if (request.Type == TransactionType.Transfer)
        {
            entity.WarehouseId = null;
        }
        else
        {
            entity.WarehouseId = request.WarehouseId;
        }

        return entity;
    }

    private void UpdateEntityFromRequest(StockTransaction entity, UpdateStockTransactionRequest request)
    {
        entity.Type = request.Type;
        entity.Quantity = request.Quantity;
        entity.TransactionDate = request.TransactionDate;
        entity.DocumentNumber = request.DocumentNumber;
        entity.Description = request.Description;
        entity.StockCardId = request.StockCardId;
        entity.FromWarehouseId = request.FromWarehouseId;
        entity.ToWarehouseId = request.ToWarehouseId;
        entity.UserId = request.UserId;

        // Transfer işleminde WarehouseId null olmalı
        if (request.Type == TransactionType.Transfer)
        {
            entity.WarehouseId = null;
        }
        else
        {
            entity.WarehouseId = request.WarehouseId;
        }
    }

    private StockTransactionDto MapToDto(StockTransaction st)
    {
        return new StockTransactionDto
        {
            Id = st.Id,
            Type = st.Type,
            Quantity = st.Quantity,
            TransactionDate = st.TransactionDate,
            DocumentNumber = st.DocumentNumber,
            Description = st.Description,
            StockCardId = st.StockCardId,
            StockCardName = st.StockCard?.Name,
            WarehouseId = st.WarehouseId,
            WarehouseName = st.Warehouse?.Name,
            FromWarehouseId = st.FromWarehouseId,
            FromWarehouseName = st.FromWarehouse?.Name,
            ToWarehouseId = st.ToWarehouseId,
            ToWarehouseName = st.ToWarehouse?.Name,
            UserId = st.UserId,
            UserFullName = st.User?.FullName
        };
    }

    private ServiceResult ValidateTransactionRequest(CreateStockTransactionRequest request)
    {
        switch (request.Type)
        {
            case TransactionType.Giris:
                if (request.WarehouseId <= 0 || request.FromWarehouseId.HasValue || request.ToWarehouseId.HasValue)
                    return ServiceResult.Fail("Giriş işlemi için yalnızca WarehouseId dolu olmalı.");
                break;

            case TransactionType.Cikis:
                if (request.WarehouseId <= 0 || request.FromWarehouseId.HasValue || request.ToWarehouseId.HasValue)
                    return ServiceResult.Fail("Çıkış işlemi için yalnızca WarehouseId dolu olmalı.");
                break;

            case TransactionType.Transfer:
                if (!request.FromWarehouseId.HasValue || !request.ToWarehouseId.HasValue || (request.WarehouseId.HasValue && request.WarehouseId > 0))
                    return ServiceResult.Fail("Transfer için FromWarehouseId ve ToWarehouseId zorunlu, WarehouseId boş olmalı.");
                break;
        }

        if (request.Quantity <= 0)
            return ServiceResult.Fail("Miktar sıfırdan büyük olmalı.");

        return ServiceResult.Success();
    }

    // Overload for UpdateStockTransactionRequest
    private ServiceResult ValidateTransactionRequest(UpdateStockTransactionRequest request)
    {
        var createRequest = new CreateStockTransactionRequest
        {
            Type = request.Type,
            Quantity = request.Quantity,
            StockCardId = request.StockCardId,
            WarehouseId = request.WarehouseId,
            FromWarehouseId = request.FromWarehouseId,
            ToWarehouseId = request.ToWarehouseId
        };

        return ValidateTransactionRequest(createRequest);
    }

    private async Task<ServiceResult> ProcessWarehouseStocks(StockTransaction entity, bool isCreate)
    {
        switch (entity.Type)
        {
            case TransactionType.Giris:
                return await ProcessIncomingStock(entity.WarehouseId.Value, entity.StockCardId, entity.Quantity);

            case TransactionType.Cikis:
                return await ProcessOutgoingStock(entity.WarehouseId.Value, entity.StockCardId, entity.Quantity);

            case TransactionType.Transfer:
                var outResult = await ProcessOutgoingStock(entity.FromWarehouseId.Value, entity.StockCardId, entity.Quantity);
                if (!outResult.IsSuccess)
                    return outResult;

                return await ProcessIncomingStock(entity.ToWarehouseId.Value, entity.StockCardId, entity.Quantity);

            default:
                return ServiceResult.Fail("Geçersiz transaction tipi.");
        }
    }

    private async Task<ServiceResult> ProcessIncomingStock(int warehouseId, int stockCardId, decimal quantity)
    {
        var stock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(warehouseId, stockCardId);
        if (stock == null)
        {
            stock = new WarehouseStock
            {
                WarehouseId = warehouseId,
                StockCardId = stockCardId,
                Quantity = 0
            };
            await _warehouseStockRepository.AddAsync(stock);
        }

        stock.Quantity += quantity;
        _warehouseStockRepository.Update(stock);
        return ServiceResult.Success();
    }

    private async Task<ServiceResult> ProcessOutgoingStock(int warehouseId, int stockCardId, decimal quantity)
    {
        var stock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(warehouseId, stockCardId);
        if (stock == null || stock.Quantity < quantity)
            return ServiceResult.Fail($"Depoda yeterli stok yok. Mevcut: {stock?.Quantity ?? 0}, İstenen: {quantity}");

        stock.Quantity -= quantity;
        _warehouseStockRepository.Update(stock);
        return ServiceResult.Success();
    }

    private async Task<ServiceResult> ReverseTransactionStocks(StockTransaction entity, bool isRollback = false)
    {
        switch (entity.Type)
        {
            case TransactionType.Giris:
                // Giriş işlemini geri al = Çıkış yap
                return await ProcessOutgoingStock(entity.WarehouseId.Value, entity.StockCardId, entity.Quantity);

            case TransactionType.Cikis:
                // Çıkış işlemini geri al = Giriş yap
                return await ProcessIncomingStock(entity.WarehouseId.Value, entity.StockCardId, entity.Quantity);

            case TransactionType.Transfer:
                // Transfer işlemini geri al
                // 1. Hedef depodan çıkar
                var outResult = await ProcessOutgoingStock(entity.ToWarehouseId.Value, entity.StockCardId, entity.Quantity);
                if (!outResult.IsSuccess && !isRollback)
                    return outResult;

                // 2. Kaynak depoya geri ekle
                return await ProcessIncomingStock(entity.FromWarehouseId.Value, entity.StockCardId, entity.Quantity);

            default:
                return ServiceResult.Fail("Geçersiz transaction tipi.");
        }
    }

    #endregion
}