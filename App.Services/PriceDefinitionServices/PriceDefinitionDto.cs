using App.Repositories.PriceDefinitions;

namespace App.Services.PriceDefinitionServices
{
    public class PriceDefinitionDto
    {
        public int Id { get; set; }
        public PriceType PriceType { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive { get; set; }  
        public int StockCardId { get; set; }  
        public string? StockCardName { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
    }
}
