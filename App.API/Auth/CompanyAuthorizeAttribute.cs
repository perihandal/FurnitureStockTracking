using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace App.API.Auth
{
    /// <summary>
    /// Şirket bazlı yetkilendirme attribute'u
    /// Editor ve User rolleri sadece kendi şirketlerinin verilerine erişebilir
    /// </summary>
    public class CompanyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Authentication kontrolü
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = context.HttpContext.User;
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // Admin rolü her şeye erişebilir
            if (roles.Contains("Admin"))
            {
                return;
            }

            // Editor ve User rolleri için CompanyId kontrolü
            var companyIdClaim = user.FindFirst("companyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim))
            {
                context.Result = new ForbidResult();
                return;
            }

            // CompanyId'yi HttpContext'e ekle (Service layer'da kullanım için)
            context.HttpContext.Items["UserCompanyId"] = int.Parse(companyIdClaim);
            
            // BranchId'yi de ekle (User rolü için)
            var branchIdClaim = user.FindFirst("branchId")?.Value;
            if (!string.IsNullOrEmpty(branchIdClaim))
            {
                context.HttpContext.Items["UserBranchId"] = int.Parse(branchIdClaim);
            }
        }
    }
}