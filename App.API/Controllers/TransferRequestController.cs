using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.API.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace App.API.Controllers
{
    [Authorize]
    [CompanyAuthorize]
    public class TransferRequestController : CustomBaseController
    {
        // Transfer request DTO'ları
        public record CreateTransferRequestDto(
            int FromWarehouseId,
            int ToWarehouseId,
            int StockCardId,
            decimal Quantity,
            string? Description
        );

        public record ApproveTransferRequestDto(
            bool IsApproved,
            string? ApprovalNotes
        );

        [HttpPost]
        public async Task<IActionResult> CreateTransferRequest([FromBody] CreateTransferRequestDto request)
        {
            // Kullanıcı bilgilerini token'dan al
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // User rolü sadece kendi deposundan çıkış yapabilir
            if (roles.Contains("User"))
            {
                var userBranchId = HttpContext.Items["UserBranchId"] as int?;
                // Burada FromWarehouse'un user'ın branch'ine ait olup olmadığını kontrol etmek gerekir
                // Bu kontrol service layer'da yapılacak
            }

            // TODO: TransferRequestService implement edilecek
            // var result = await transferRequestService.CreateAsync(request, userId);
            // return CreateActionResult(result);
            
            return Ok(new { message = "Transfer request created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetTransferRequests()
        {
            // Kullanıcının rolüne göre transfer request'leri getir
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
            // TODO: TransferRequestService implement edilecek
            // var result = await transferRequestService.GetByUserRoleAsync(userId, roles);
            // return CreateActionResult(result);
            
            return Ok(new { message = "Transfer requests retrieved successfully" });
        }

        [RoleAuthorize("Admin", "Editor")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveTransferRequest(int id, [FromBody] ApproveTransferRequestDto request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            // TODO: TransferRequestService implement edilecek
            // var result = await transferRequestService.ApproveAsync(id, userId, request.IsApproved, request.ApprovalNotes);
            // return CreateActionResult(result);
            
            return Ok(new { message = "Transfer request processed successfully" });
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTransferRequests()
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
            // User rolü sadece kendi request'lerini görebilir
            // Editor/Admin onaylanmayı bekleyen tüm request'leri görebilir
            
            // TODO: TransferRequestService implement edilecek
            return Ok(new { message = "Pending transfer requests retrieved successfully" });
        }
    }
}