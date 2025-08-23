using App.Repositories.BarcodeCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.BarcodeCardServices
{
    public class BarcodeCardDto
    {
        public int Id { get; set; }
        public string BarcodeCode { get; set; } = default!;
        public BarcodeType BarcodeType { get; set; }
        public bool IsDefault { get; set; }
        public int StockCardId { get; set; }
        public string? StockCardName { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}
