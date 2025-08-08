using App.Repositories.PriceDefinitions;

namespace App.Services.PriceDefinitionServices
{
    public class CreatePriceDefinitionRequest
    {
        public PriceType PriceType { get; set; }  // Fiyat tipi (Alış, Satış, vb.)
        public decimal Price { get; set; }  // Fiyat miktarı
        public Currency Currency { get; set; } = Currency.TRY;  // Para birimi (Varsayılan TRY)
        public DateTime ValidFrom { get; set; }  // Fiyatın geçerlilik başlangıcı
        public DateTime? ValidTo { get; set; }  // Fiyatın geçerlilik bitişi (nullable)
        public int UserId { get; set; }  // Fiyatı belirleyen kullanıcı ID'si
        public int StockCardId { get; set; }  // Fiyatın ait olduğu Stok Kartı ID'si
    }
}
