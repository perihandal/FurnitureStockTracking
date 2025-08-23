using App.Repositories.BarcodeCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool ValidateBarcode(string barcodeCode, BarcodeType barcodeType)
        {
            if (string.IsNullOrWhiteSpace(barcodeCode))
                return false;

            return barcodeType switch
            {
                BarcodeType.EAN13 => ValidateEAN13(barcodeCode),
                BarcodeType.UPC_A => ValidateUPCA(barcodeCode),
                BarcodeType.EAN8 => ValidateEAN8(barcodeCode),
                BarcodeType.Code128 => ValidateCode128(barcodeCode),
                BarcodeType.ITF14 => ValidateITF14(barcodeCode),
                BarcodeType.ISBN => ValidateISBN(barcodeCode),
                BarcodeType.QRCode => ValidateQRCode(barcodeCode),
                BarcodeType.DataMatrix => ValidateDataMatrix(barcodeCode),
                _ => false
            };
        }

        #region EAN13 Generation and Validation
        private string GenerateEAN13(int stockCardId, int companyId)
        {
            // EAN13: 13 haneli, son hane check digit
            var companyPrefix = companyId.ToString("D3").Substring(0, 3);
            var itemCode = stockCardId.ToString("D9");
            var withoutCheckDigit = companyPrefix + itemCode;
            var checkDigit = CalculateEAN13CheckDigit(withoutCheckDigit);
            return withoutCheckDigit + checkDigit;
        }

        private bool ValidateEAN13(string code)
        {
            if (code.Length != 13 || !code.All(char.IsDigit))
                return false;

            var withoutCheckDigit = code.Substring(0, 12);
            var expectedCheckDigit = CalculateEAN13CheckDigit(withoutCheckDigit);
            var actualCheckDigit = int.Parse(code[12].ToString());

            return expectedCheckDigit == actualCheckDigit;
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

        #region UPC-A Generation and Validation
        private string GenerateUPCA(int stockCardId, int companyId)
        {
            // UPC-A: 12 haneli, son hane check digit
            var companyPrefix = companyId.ToString("D2").Substring(0, 2);
            var itemCode = stockCardId.ToString("D9");
            var withoutCheckDigit = companyPrefix + itemCode;
            var checkDigit = CalculateUPCCheckDigit(withoutCheckDigit);
            return withoutCheckDigit + checkDigit;
        }

        private bool ValidateUPCA(string code)
        {
            if (code.Length != 12 || !code.All(char.IsDigit))
                return false;

            var withoutCheckDigit = code.Substring(0, 11);
            var expectedCheckDigit = CalculateUPCCheckDigit(withoutCheckDigit);
            var actualCheckDigit = int.Parse(code[11].ToString());

            return expectedCheckDigit == actualCheckDigit;
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

        #region EAN8 Generation and Validation
        private string GenerateEAN8(int stockCardId)
        {
            // EAN8: 8 haneli, son hane check digit
            var itemCode = stockCardId.ToString("D7");
            var checkDigit = CalculateEAN8CheckDigit(itemCode);
            return itemCode + checkDigit;
        }

        private bool ValidateEAN8(string code)
        {
            if (code.Length != 8 || !code.All(char.IsDigit))
                return false;

            var withoutCheckDigit = code.Substring(0, 7);
            var expectedCheckDigit = CalculateEAN8CheckDigit(withoutCheckDigit);
            var actualCheckDigit = int.Parse(code[7].ToString());

            return expectedCheckDigit == actualCheckDigit;
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

        #region Code128 Generation and Validation
        private string GenerateCode128(int stockCardId, int companyId)
        {
            // Code128: Alfanümerik, değişken uzunluk
            return $"C{companyId:D4}S{stockCardId:D8}";
        }

        private bool ValidateCode128(string code)
        {
            // Code128 format: C{4 digit company}S{8 digit stock}
            if (code.Length != 14 || !code.StartsWith("C") || !code.Contains("S"))
                return false;

            var parts = code.Split('S');
            if (parts.Length != 2)
                return false;

            var companyPart = parts[0].Substring(1); // Remove 'C'
            var stockPart = parts[1];

            return companyPart.Length == 4 && companyPart.All(char.IsDigit) &&
                   stockPart.Length == 8 && stockPart.All(char.IsDigit);
        }
        #endregion

        #region ITF14 Generation and Validation
        private string GenerateITF14(int stockCardId, int companyId)
        {
            // ITF14: 14 haneli, genellikle EAN13'ün başına 0 eklenir
            var ean13 = GenerateEAN13(stockCardId, companyId);
            return "0" + ean13;
        }

        private bool ValidateITF14(string code)
        {
            if (code.Length != 14 || !code.All(char.IsDigit))
                return false;

            // ITF14 genellikle 0 ile başlar ve EAN13 içerir
            if (code.StartsWith("0"))
            {
                var ean13Part = code.Substring(1);
                return ValidateEAN13(ean13Part);
            }

            return true; // Diğer ITF14 formatları için basit kontrol
        }
        #endregion

        #region ISBN Generation and Validation
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

        private bool ValidateISBN(string code)
        {
            if (code.Length != 13 || !code.All(char.IsDigit))
                return false;

            // ISBN-13 978 veya 979 ile başlamalı
            if (!code.StartsWith("978") && !code.StartsWith("979"))
                return false;

            return ValidateEAN13(code);
        }
        #endregion

        #region QRCode Generation and Validation
        private string GenerateQRCode(int stockCardId, int companyId, string timestamp)
        {
            // QR Code: JSON formatında veri
            return $"{{\"stockId\":{stockCardId},\"companyId\":{companyId},\"timestamp\":\"{timestamp}\"}}";
        }

        private bool ValidateQRCode(string code)
        {
            // QR Code için basit JSON format kontrolü
            try
            {
                return code.StartsWith("{") && code.EndsWith("}") &&
                       code.Contains("\"stockId\":") &&
                       code.Contains("\"companyId\":") &&
                       code.Contains("\"timestamp\":");
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DataMatrix Generation and Validation
        private string GenerateDataMatrix(int stockCardId, int companyId, string timestamp)
        {
            // DataMatrix: Kompakt format
            return $"DM{companyId:D4}{stockCardId:D8}{timestamp}";
        }

        private bool ValidateDataMatrix(string code)
        {
            // DataMatrix format: DM{4 digit company}{8 digit stock}{timestamp}
            if (!code.StartsWith("DM") || code.Length < 14)
                return false;

            var companyPart = code.Substring(2, 4);
            var stockPart = code.Substring(6, 8);

            return companyPart.All(char.IsDigit) && stockPart.All(char.IsDigit);
        }
        #endregion
    }
}
