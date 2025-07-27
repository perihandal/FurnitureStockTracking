using App.Repositories.StockCards;
using App.Repositories.SubGroups;

namespace App.Repositories.MainGroups
{
    public class MainGroup
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<SubGroup> SubGroups { get; set; } = new List<SubGroup>();
        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
    }
}
