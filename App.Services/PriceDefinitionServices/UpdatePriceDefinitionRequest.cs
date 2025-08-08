using App.Repositories.PriceDefinitions;

namespace App.Services.PriceDefinitionServices
{
    public class UpdatePriceDefinitionRequest
    {
        public PriceType PriceType { get; set; }  // Fiyat tipi (Alış, Satış, vb.)
        public decimal Price { get; set; }  // Yeni fiyat miktarı
        public Currency Currency { get; set; } = Currency.TRY;  // Yeni para birimi
        public DateTime ValidFrom { get; set; }  // Fiyatın yeni geçerlilik başlangıcı
        public DateTime? ValidTo { get; set; }  // Fiyatın yeni geçerlilik bitişi (nullable)
        public int UserId { get; set; }  // Fiyatı güncelleyen kullanıcı ID'si
    }
}
