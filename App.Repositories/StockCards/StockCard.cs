using App.Repositories.Categories;
using App.Repositories.MainGroups;
using App.Repositories.SubGroups;
using App.Repositories.Companies;
using App.Repositories.BarcodeCards;
using App.Repositories.PriceDefinitions;
using App.Repositories.StockTransactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using App.Repositories.Branches;
using App.Repositories.Users;
using App.Repositories.WarehouseStocks;

namespace App.Repositories.StockCards
{
    public enum StockCardType
    {
        NihaiUrun,
        AraUrun,
        Hammadde
    }

    public class StockCard
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public StockCardType Type { get; set; } = default!;
        public string Unit { get; set; } = default!;
        public decimal Tax { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = default!;

        public int MainGroupId { get; set; }
        public MainGroup MainGroup { get; set; } = default!;

        public int? SubGroupId { get; set; }
        public SubGroup? SubGroup { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }


        public ICollection<BarcodeCard> BarcodeCards { get; set; } = new List<BarcodeCard>();
        public ICollection<PriceDefinition> PriceDefinitions { get; set; } = new List<PriceDefinition>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    }
}
