using App.Repositories.StockCards;
using App.Repositories.Warehouses;

namespace App.Repositories.WarehouseStocks
{
    public class WarehouseStock
    {
        public int Id { get; set; }

        // Depo
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        // Ürün
        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; }

        // Miktar
        public decimal Quantity { get; set; }
    }
}
