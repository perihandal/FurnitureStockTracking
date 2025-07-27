using App.Repositories.StockCards;
using App.Repositories.Warehouses;

namespace App.Repositories.StockTransactions
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public string TransactionType { get; set; } = default!; // "Giris", "Cikis", "Transfer"
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }

        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; } = default!;

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;
    }
}
