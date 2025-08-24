using App.Repositories.BarcodeCards;

namespace App.Services.BarcodeCardValidationService
{
    public interface IBarcodeValidationService
    {
        bool ValidateBarcode(string barcodeCode, BarcodeType barcodeType);
        bool ValidateEAN13(string code);
        bool ValidateUPCA(string code);
        bool ValidateEAN8(string code);
        bool ValidateCode128(string code);
        bool ValidateITF14(string code);
        bool ValidateISBN(string code);
        bool ValidateQRCode(string code);
        bool ValidateDataMatrix(string code);
    }
}