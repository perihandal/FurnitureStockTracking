using App.Repositories.StockCards;
using App.Repositories.BarcodeCards;
using System;
using System.ComponentModel.DataAnnotations;

namespace App.Services.StockCardServices
{
    public record class CreateStockCardRequest(
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        string Name,
        
        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        string Code,
        
        [Required(ErrorMessage = "Type is required")]
        StockCardType Type,
        
        [Required(ErrorMessage = "Unit is required")]
        [StringLength(20, ErrorMessage = "Unit cannot exceed 20 characters")]
        string Unit,
        
        [Required(ErrorMessage = "Tax is required")]
        [Range(0, 100, ErrorMessage = "Tax must be between 0 and 100")]
        decimal Tax,
        
        [Required(ErrorMessage = "CompanyId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CompanyId must be greater than 0")]
        int CompanyId,
        
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        int UserId,
        
        [Required(ErrorMessage = "BranchId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be greater than 0")]
        int BranchId,
        
        [Required(ErrorMessage = "MainGroupId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MainGroupId must be greater than 0")]
        int MainGroupId,
        
        int? SubGroupId,
        
        int? CategoryId,
        
        bool CreateDefaultBarcode = true,
        
        BarcodeType DefaultBarcodeType = BarcodeType.EAN13
    );
}