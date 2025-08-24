using App.Repositories.BarcodeCards;

namespace App.Services.BarcodeCardGeneratorService
{
    public class BarcodeGeneratorService : IBarcodeGeneratorService
    {
        public string GenerateBarcode(BarcodeType barcodeType, int stockCardId, int companyId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            return barcodeType switch
            {
                BarcodeType.EAN13 => GenerateEAN13(stockCardId, companyId),
                BarcodeType.UPC_A => GenerateUPCA(stockCardId, companyId),
                BarcodeType.EAN8 => GenerateEAN8(stockCardId),
                BarcodeType.Code128 => GenerateCode128(stockCardId, companyId),
                BarcodeType.ITF14 => GenerateITF14(stockCardId, companyId),
                BarcodeType.ISBN => GenerateISBN(stockCardId),
                BarcodeType.QRCode => GenerateQRCode(stockCardId, companyId, timestamp),
                BarcodeType.DataMatrix => GenerateDataMatrix(stockCardId, companyId, timestamp),
                _ => throw new ArgumentException($"Unsupported barcode type: {barcodeType}")
            };
        }

        public string GenerateUniqueBarcode(BarcodeType barcodeType, int stockCardId, int companyId, Func<string, bool> existsCheck, int maxAttempts = 100)
        {
            string barcodeCode;
            int attempts = 0;

            do
            {
                barcodeCode = GenerateBarcode(barcodeType, stockCardId, companyId);
                attempts++;

                if (attempts >= maxAttempts)
                {
                    throw new InvalidOperationException($"Unable to generate unique barcode after {maxAttempts} attempts for type {barcodeType}");
                }
            }
            while (existsCheck(barcodeCode));

            return barcodeCode;
        }

        #region EAN13 Generation
        private string GenerateEAN13(int stockCardId, int companyId)
        {
            // EAN13: 13 haneli, son hane check digit
            var companyPrefix = companyId.ToString("D3").Substring(0, 3);
            var itemCode = stockCardId.ToString("D9");
            var withoutCheckDigit = companyPrefix + itemCode;
            var checkDigit = CalculateEAN13CheckDigit(withoutCheckDigit);
            return withoutCheckDigit + checkDigit;
        }

        private int CalculateEAN13CheckDigit(string code)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(code[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }
        #endregion

        #region UPC-A Generation
        private string GenerateUPCA(int stockCardId, int companyId)
        {
            // UPC-A: 12 haneli, son hane check digit
            var companyPrefix = companyId.ToString("D2").Substring(0, 2);
            var itemCode = stockCardId.ToString("D9");
            var withoutCheckDigit = companyPrefix + itemCode;
            var checkDigit = CalculateUPCCheckDigit(withoutCheckDigit);
            return withoutCheckDigit + checkDigit;
        }

        private int CalculateUPCCheckDigit(string code)
        {
            int sum = 0;
            for (int i = 0; i < 11; i++)
            {
                int digit = int.Parse(code[i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }
        #endregion

        #region EAN8 Generation
        private string GenerateEAN8(int stockCardId)
        {
            // EAN8: 8 haneli, son hane check digit
            var itemCode = stockCardId.ToString("D7");
            var checkDigit = CalculateEAN8CheckDigit(itemCode);
            return itemCode + checkDigit;
        }

        private int CalculateEAN8CheckDigit(string code)
        {
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                int digit = int.Parse(code[i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }
        #endregion

        #region Code128 Generation
        private string GenerateCode128(int stockCardId, int companyId)
        {
            // Code128: Alfanümerik, değişken uzunluk
            return $"C{companyId:D4}S{stockCardId:D8}";
        }
        #endregion

        #region ITF14 Generation
        private string GenerateITF14(int stockCardId, int companyId)
        {
            // ITF14: 14 haneli, genellikle EAN13'ün başına 0 eklenir
            var ean13 = GenerateEAN13(stockCardId, companyId);
            return "0" + ean13;
        }
        #endregion

        #region ISBN Generation
        private string GenerateISBN(int stockCardId)
        {
            // ISBN: 13 haneli (ISBN-13 formatında)
            var prefix = "978";
            var group = "0"; // İngilizce grup
            var publisher = stockCardId.ToString("D3");
            var title = stockCardId.ToString("D6");
            var withoutCheckDigit = prefix + group + publisher + title;
            var checkDigit = CalculateEAN13CheckDigit(withoutCheckDigit);
            return withoutCheckDigit + checkDigit;
        }
        #endregion

        #region QRCode Generation
        private string GenerateQRCode(int stockCardId, int companyId, string timestamp)
        {
            // QR Code: JSON formatında veri
            return $"{{\"stockId\":{stockCardId},\"companyId\":{companyId},\"timestamp\":\"{timestamp}\"}}";
        }
        #endregion

        #region DataMatrix Generation
        private string GenerateDataMatrix(int stockCardId, int companyId, string timestamp)
        {
            // DataMatrix: Kompakt format
            return $"DM{companyId:D4}{stockCardId:D8}{timestamp}";
        }
        #endregion
    }
}