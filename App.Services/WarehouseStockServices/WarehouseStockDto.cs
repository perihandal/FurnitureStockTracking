public class WarehouseStockDto
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = default!;
    public int StockCardId { get; set; }
    public string StockCardName { get; set; } = default!;
    public decimal Quantity { get; set; }
}
