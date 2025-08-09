using App.Repositories;
using App.Repositories.StockTransactions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.Services.StockTransactionServices
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StockTransactionService(IStockTransactionRepository stockTransactionRepository, IUnitOfWork unitOfWork)
        {
            _stockTransactionRepository = stockTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<List<StockTransactionDto>>> GetAllListAsync()
        {
            var transactions = await _stockTransactionRepository.GetAllWithDetailsAsync();

            var transactionDtos = transactions.Select(tr => new StockTransactionDto
            {
                Id = tr.Id,
                Type = tr.Type,
                Quantity = tr.Quantity,
                TransactionDate = tr.TransactionDate,
                DocumentNumber = tr.DocumentNumber,
                Description = tr.Description,
                StockCardName = tr.StockCard.Name,
                WarehouseName = tr.Warehouse.Name,
                FromWarehouseName = tr.FromWarehouse?.Name,
                ToWarehouseName = tr.ToWarehouse?.Name,
                UserFullName = tr.User?.FullName
            }).ToList();

            return ServiceResult<List<StockTransactionDto>>.Success(transactionDtos);
        }

        public async Task<ServiceResult<StockTransactionDto?>> GetByIdAsync(int id)
        {
            var transaction = await _stockTransactionRepository.GetByIdWithDetailsAsync(id);

            if (transaction == null)
                return ServiceResult<StockTransactionDto?>.Fail("Stock transaction not found", HttpStatusCode.NotFound);

            var dto = new StockTransactionDto
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                TransactionDate = transaction.TransactionDate,
                DocumentNumber = transaction.DocumentNumber,
                Description = transaction.Description,
                StockCardName = transaction.StockCard.Name,
                WarehouseName = transaction.Warehouse.Name,
                FromWarehouseName = transaction.FromWarehouse?.Name,
                ToWarehouseName = transaction.ToWarehouse?.Name,
                UserFullName = transaction.User?.FullName
            };

            return ServiceResult<StockTransactionDto?>.Success(dto);
        }

        public async Task<ServiceResult<CreateStockTransactionResponse>> CreateAsync(CreateStockTransactionRequest request)
        {
            var transaction = new StockTransaction
            {
                Type = request.Type,
                Quantity = request.Quantity,
                TransactionDate = request.TransactionDate,
                DocumentNumber = request.DocumentNumber,
                Description = request.Description,
                StockCardId = request.StockCardId,
                WarehouseId = request.WarehouseId,
                FromWarehouseId = request.FromWarehouseId,
                ToWarehouseId = request.ToWarehouseId,
                UserId = request.UserId
            };

            await _stockTransactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateStockTransactionResponse>.Success(new CreateStockTransactionResponse(transaction.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateStockTransactionRequest request)
        {
            var transaction = await _stockTransactionRepository.GetByIdAsync(id);

            if (transaction == null)
                return ServiceResult.Fail("Stock transaction not found", HttpStatusCode.NotFound);

            transaction.Type = request.Type;
            transaction.Quantity = request.Quantity;
            transaction.TransactionDate = request.TransactionDate;
            transaction.DocumentNumber = request.DocumentNumber;
            transaction.Description = request.Description;
            transaction.StockCardId = request.StockCardId;
            transaction.WarehouseId = request.WarehouseId;
            transaction.FromWarehouseId = request.FromWarehouseId;
            transaction.ToWarehouseId = request.ToWarehouseId;
            transaction.UserId = request.UserId;

            _stockTransactionRepository.Update(transaction);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var transaction = await _stockTransactionRepository.GetByIdAsync(id);

            if (transaction == null)
                return ServiceResult.Fail("Stock transaction not found", HttpStatusCode.NotFound);

            _stockTransactionRepository.Delete(transaction);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
