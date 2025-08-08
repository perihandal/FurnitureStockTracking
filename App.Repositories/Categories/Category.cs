using App.Repositories.StockCards;
using App.Repositories.Branches;
using App.Repositories.Users;
using App.Repositories.Companies;

namespace App.Repositories.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public int BranchId { get; set; }
        public int CompanyId { get; set; }

        // Navigation Properties
        public Branch Branch { get; set; } = default!;
        public Company Company { get; set; } = default!;
        public User? User { get; set; }
        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
    }
}
