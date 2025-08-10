public class StockTransactionDto
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Description { get; set; }

    public int StockCardId { get; set; }
    public string StockCardName { get; set; } = default!;

    public int? WarehouseId { get; set; }
    public string? WarehouseName { get; set; } = default!;

    public int? FromWarehouseId { get; set; }
    public string? FromWarehouseName { get; set; }

    public int? ToWarehouseId { get; set; }
    public string? ToWarehouseName { get; set; }

    public int? UserId { get; set; }
    public string? UserFullName { get; set; }
}
