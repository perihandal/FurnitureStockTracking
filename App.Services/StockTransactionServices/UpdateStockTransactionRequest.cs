namespace App.Services.StockTransactionServices
{
    public class UpdateStockTransactionRequest
    {
        public TransactionType Type { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }
        public int StockCardId { get; set; }
        public int WarehouseId { get; set; }
        public int? FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public int? UserId { get; set; }
    }
}