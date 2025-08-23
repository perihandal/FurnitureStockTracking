using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.BarcodeCardServices
{
    public class UpdateBarcodeCardRequest
    {
        public bool IsDefault { get; set; }
        public int? UserId { get; set; }
        public int BranchId { get; set; }
    }
}
