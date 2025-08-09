namespace App.Services.StockTransactionServices
{
    public class StockTransactionDto
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }
        public string StockCardName { get; set; } = default!;
        public string WarehouseName { get; set; } = default!;
        public string? FromWarehouseName { get; set; }
        public string? ToWarehouseName { get; set; }
        public string? UserFullName { get; set; }
    }
}