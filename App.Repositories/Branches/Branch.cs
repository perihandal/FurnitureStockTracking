using App.Repositories.BarcodeCards;
using App.Repositories.Companies;
using App.Repositories.StockCards;
using App.Repositories.Users;
using App.Repositories.Warehouses;

namespace App.Repositories.Branches
{
    public class Branch
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public int? UserId { get; set; }
        public User? User { get; set; }


        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public ICollection<BarcodeCard> BarcodeCards { get; set; } = new List<BarcodeCard>();
    }
}
