using App.Repositories.StockCards;

namespace App.Repositories.BarcodeCards
{
    public class BarcodeCard
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = default!;
        public string BarcodeType { get; set; } = default!;
        public bool IsDefault { get; set; } = false;

        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; } = default!;
    }
}
