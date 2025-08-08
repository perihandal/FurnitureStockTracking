using App.Repositories.StockCards;
using App.Repositories.Users;

namespace App.Repositories.PriceDefinitions
{
    public enum PriceType
    {
        Alış = 1,
        Satış = 2,
        İndirimli = 3,
        Normal = 4
    }

    public enum Currency
    {
        TRY = 1,
        USD = 2,
        EUR = 3,
        GBP = 4
    }
    public class PriceDefinition
    {
        public int Id { get; set; }

        public PriceType PriceType { get; set; }

        public decimal Price { get; set; }

        public Currency Currency { get; set; } = Currency.TRY;

        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
        public DateTime? ValidTo { get; set; }

        // Fiyat geçerlilik durumunu kontrol eden sanal alan
        public bool IsActive { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; } 

        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; } = default!;
    }
}
