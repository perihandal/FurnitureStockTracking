using App.Repositories.BarcodeCards;

namespace App.Services.BarcodeCardValidationService
{
    public class BarcodeValidationService : IBarcodeValidationService
    {
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

        #region EAN13 Validation
        public bool ValidateEAN13(string code)
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

        #region UPC-A Validation
        public bool ValidateUPCA(string code)
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

        #region EAN8 Validation
        public bool ValidateEAN8(string code)
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

        #region Code128 Validation
        public bool ValidateCode128(string code)
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

        #region ITF14 Validation
        public bool ValidateITF14(string code)
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

        #region ISBN Validation
        public bool ValidateISBN(string code)
        {
            if (code.Length != 13 || !code.All(char.IsDigit))
                return false;

            // ISBN-13 978 veya 979 ile başlamalı
            if (!code.StartsWith("978") && !code.StartsWith("979"))
                return false;

            return ValidateEAN13(code);
        }
        #endregion

        #region QRCode Validation
        public bool ValidateQRCode(string code)
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

        #region DataMatrix Validation
        public bool ValidateDataMatrix(string code)
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