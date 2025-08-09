namespace App.Services.PriceDefinitionServices
{
    public class PriceHistoryDto
    {
        public string StockCardName { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangeDate { get; set; }
        
    }
}