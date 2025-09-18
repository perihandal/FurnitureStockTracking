using App.Repositories.PriceDefinitions;
using System.ComponentModel.DataAnnotations;

namespace App.Services.PriceDefinitionServices
{
    public class CreatePriceDefinitionRequest
    {
        [Required(ErrorMessage = "PriceType is required")]
        public PriceType PriceType { get; set; }  // Fiyat tipi (Alış, Satış, vb.)
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }  // Fiyat miktarı
        
        [Required(ErrorMessage = "Currency is required")]
        public Currency Currency { get; set; } = Currency.TRY;  // Para birimi (Varsayılan TRY)
        
        [Required(ErrorMessage = "ValidFrom is required")]
        public DateTime ValidFrom { get; set; }  // Fiyatın geçerlilik başlangıcı
        
        public DateTime? ValidTo { get; set; }  // Fiyatın geçerlilik bitişi (nullable)
        
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }  // Fiyatı belirleyen kullanıcı ID'si
        
        [Required(ErrorMessage = "StockCardId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "StockCardId must be greater than 0")]
        public int StockCardId { get; set; }  // Fiyatın ait olduğu Stok Kartı ID'si
    }
}
