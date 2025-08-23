using App.Repositories.BarcodeCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.BarcodeCardGeneratorService
{
    public interface IBarcodeGeneratorService
    {
        string GenerateBarcode(BarcodeType barcodeType, int stockCardId, int companyId);
        bool ValidateBarcode(string barcodeCode, BarcodeType barcodeType);
        string GenerateUniqueBarcode(BarcodeType barcodeType, int stockCardId, int companyId, Func<string, bool> existsCheck, int maxAttempts = 100);
    }
}
