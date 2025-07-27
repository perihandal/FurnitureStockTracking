using App.Repositories.StockCards;

namespace App.Repositories.PriceDefinitions
{
    public class PriceDefinition
    {
        public int Id { get; set; }
        public string PriceType { get; set; } = default!; // Alış, Satış, vs.
        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";
        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
        public DateTime? ValidTo { get; set; }

        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; } = default!;
    }
}
