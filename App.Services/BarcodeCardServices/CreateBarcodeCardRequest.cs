using App.Repositories.BarcodeCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.BarcodeCardServices
{
    public class CreateBarcodeCardRequest
    {
        public BarcodeType BarcodeType { get; set; }
        public bool IsDefault { get; set; }
        public int StockCardId { get; set; }
        public int? UserId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}
