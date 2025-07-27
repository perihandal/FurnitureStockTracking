using App.Repositories.MainGroups;
using App.Repositories.StockCards;

namespace App.Repositories.SubGroups
{
    public class SubGroup
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public int MainGroupId { get; set; }
        public MainGroup MainGroup { get; set; } = default!;

        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
    }
}
