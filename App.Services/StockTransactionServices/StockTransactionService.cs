using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Repositories.StockTransactions;
using App.Repositories;
using App.Repositories.WarehouseStocks;
using App.Services;
using App.Services.StockTransactionServices;
using System.Numerics;
using System;

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
        var dtos = transactions.Select(st => new StockTransactionDto
        {
            Id = st.Id,
            Type = st.Type,
            Quantity = st.Quantity,
            TransactionDate = st.TransactionDate,
            DocumentNumber = st.DocumentNumber,
            Description = st.Description,
            StockCardId = st.StockCardId,
            StockCardName = st.StockCard.Name,
            WarehouseId = st.WarehouseId,
            WarehouseName = st.Warehouse.Name,
            FromWarehouseId = st.FromWarehouseId,
            FromWarehouseName = st.FromWarehouse?.Name,
            ToWarehouseId = st.ToWarehouseId,
            ToWarehouseName = st.ToWarehouse?.Name,
            UserId = st.UserId,
            UserFullName = st.User?.FullName
        }).ToList();

        return ServiceResult<List<StockTransactionDto>>.Success(dtos);
    }

    public async Task<ServiceResult<StockTransactionDto?>> GetByIdAsync(int id)
    {
        var st = await _stockTransactionRepository.GetByIdWithDetailsAsync(id);
        if (st == null)
            return ServiceResult<StockTransactionDto?>.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        var dto = new StockTransactionDto
        {
            Id = st.Id,
            Type = st.Type,
            Quantity = st.Quantity,
            TransactionDate = st.TransactionDate,
            DocumentNumber = st.DocumentNumber,
            Description = st.Description,
            StockCardId = st.StockCardId,
            StockCardName = st.StockCard.Name,
            WarehouseId = st.WarehouseId,
            WarehouseName = st.Warehouse.Name,
            FromWarehouseId = st.FromWarehouseId,
            FromWarehouseName = st.FromWarehouse?.Name,
            ToWarehouseId = st.ToWarehouseId,
            ToWarehouseName = st.ToWarehouse?.Name,
            UserId = st.UserId,
            UserFullName = st.User?.FullName
        };

        return ServiceResult<StockTransactionDto?>.Success(dto);
    }

    public async Task<ServiceResult<CreateStockTransactionResponse>> CreateAsync(CreateStockTransactionRequest request)
    {
        // 1. İşlem tipine göre depo ID doğrulama
        switch (request.Type)
        {
            case TransactionType.Giris:
                if (request.WarehouseId <= 0 || request.FromWarehouseId.HasValue || request.ToWarehouseId.HasValue)
                    return ServiceResult<CreateStockTransactionResponse>.Fail("Giriş işlemi için yalnızca WarehouseId dolu olmalı.");
                break;

            case TransactionType.Cikis:
                if (request.WarehouseId <= 0 || request.FromWarehouseId.HasValue || request.ToWarehouseId.HasValue)
                    return ServiceResult<CreateStockTransactionResponse>.Fail("Çıkış işlemi için yalnızca WarehouseId dolu olmalı.");
                break;

            case TransactionType.Transfer:
                if (!request.FromWarehouseId.HasValue || !request.ToWarehouseId.HasValue || request.WarehouseId != 0)
                    return ServiceResult<CreateStockTransactionResponse>.Fail("Transfer için FromWarehouseId ve ToWarehouseId zorunlu, WarehouseId boş olmalı.");
                break;
        }

        // 2. Transaction entity oluşturma
        var entity = new StockTransaction
        {
            Type = request.Type,
            Quantity = request.Quantity,
            TransactionDate = request.TransactionDate,
            DocumentNumber = request.DocumentNumber,
            Description = request.Description,
            StockCardId = request.StockCardId,
            WarehouseId = request?.WarehouseId,
            FromWarehouseId = request.FromWarehouseId,
            ToWarehouseId = request.ToWarehouseId,
            UserId = request.UserId
        };

        await _stockTransactionRepository.AddAsync(entity);

        // Transfer işleminde WarehouseId null olmalı
        if (request.Type == TransactionType.Transfer)
        {
            entity.WarehouseId = null;
        }
        else
        {
            entity.WarehouseId = request.WarehouseId;
        }


        // 3. Depo stoklarını güncelleme
        switch (entity.Type)
        {
            case TransactionType.Giris:
                var inStock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(entity.WarehouseId.Value, entity.StockCardId);
                if (inStock == null)
                {
                    inStock = new WarehouseStock
                    {
                        WarehouseId = entity.WarehouseId.Value,
                        StockCardId = entity.StockCardId,
                        Quantity = 0
                    };
                    await _warehouseStockRepository.AddAsync(inStock);
                }
                inStock.Quantity += entity.Quantity;
                _warehouseStockRepository.Update(inStock);
                break;

            case TransactionType.Cikis:
                var outStock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(entity.WarehouseId.Value, entity.StockCardId);
                if (outStock == null || outStock.Quantity < entity.Quantity)
                    return ServiceResult<CreateStockTransactionResponse>.Fail("Depoda yeterli stok yok.");
                outStock.Quantity -= entity.Quantity;
                _warehouseStockRepository.Update(outStock);
                break;

            case TransactionType.Transfer:
                var fromStock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(entity.FromWarehouseId.Value, entity.StockCardId);
                if (fromStock == null || fromStock.Quantity < entity.Quantity)
                    return ServiceResult<CreateStockTransactionResponse>.Fail("Çıkış deposunda yeterli stok yok.");
                fromStock.Quantity -= entity.Quantity;
                _warehouseStockRepository.Update(fromStock);

                var toStock = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(entity.ToWarehouseId.Value, entity.StockCardId);
                if (toStock == null)
                {
                    toStock = new WarehouseStock
                    {
                        WarehouseId = entity.ToWarehouseId.Value,
                        StockCardId = entity.StockCardId,
                        Quantity = 0
                    };
                    await _warehouseStockRepository.AddAsync(toStock);
                }
                toStock.Quantity += entity.Quantity;
                _warehouseStockRepository.Update(toStock);
                break;
        }

        // 4. Kaydet
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<CreateStockTransactionResponse>.Success(new CreateStockTransactionResponse(entity.Id));
    }


    public async Task<ServiceResult> UpdateAsync(int id, UpdateStockTransactionRequest request)
    {
        var entity = await _stockTransactionRepository.GetByIdAsync(id);
        if (entity == null)
            return ServiceResult.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        entity.Type = request.Type;
        entity.Quantity = request.Quantity;
        entity.TransactionDate = request.TransactionDate;
        entity.DocumentNumber = request.DocumentNumber;
        entity.Description = request.Description;
        entity.StockCardId = request.StockCardId;
        entity.WarehouseId = request.WarehouseId;
        entity.FromWarehouseId = request.FromWarehouseId;
        entity.ToWarehouseId = request.ToWarehouseId;
        entity.UserId = request.UserId;

        _stockTransactionRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var entity = await _stockTransactionRepository.GetByIdAsync(id);
        if (entity == null)
            return ServiceResult.Fail("StockTransaction not found", HttpStatusCode.NotFound);

        _stockTransactionRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
