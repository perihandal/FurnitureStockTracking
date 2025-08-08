using App.Repositories.StockCards;
using App.Repositories.Users;
using App.Repositories.Warehouses;

public enum TransactionType
{
    Giris,
    Cikis,
    Transfer
}

public class StockTransaction
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Quantity { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? DocumentNumber { get; set; }
    public string? Description { get; set; }

    public int StockCardId { get; set; }
    public StockCard StockCard { get; set; } = default!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public int? FromWarehouseId { get; set; }
    public Warehouse? FromWarehouse { get; set; }

    public int? ToWarehouseId { get; set; }
    public Warehouse? ToWarehouse { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }
}
