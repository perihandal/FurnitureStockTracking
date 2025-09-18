using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Services.CompanyServices
{
    public record class CreateCompanyRequest(
        [Required(ErrorMessage = "Code is required")]
        [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
        string Code,
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        string Name,
        
        [Required(ErrorMessage = "TaxNumber is required")]
        [StringLength(20, ErrorMessage = "TaxNumber cannot exceed 20 characters")]
        string TaxNumber,
        
        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        string Address,
        
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        string Phone,
        
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        int UserId,
        
        [Required(ErrorMessage = "IsActive is required")]
        bool IsActive
    );
}

