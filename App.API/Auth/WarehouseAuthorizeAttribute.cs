using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace App.API.Auth
{
    /// <summary>
    /// Depo bazlı yetkilendirme attribute'u
    /// User rolü sadece kendi şubesindeki depolara erişebilir
    /// </summary>
    public class WarehouseAuthorizeAttribute : Attribute, IAuthorizationFilter
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

            // Admin rolü her depoye erişebilir
            if (roles.Contains("Admin"))
            {
                return;
            }

            // Editor rolü kendi şirketindeki tüm depolara erişebilir
            if (roles.Contains("Editor"))
            {
                var companyIdClaim = user.FindFirst("companyId")?.Value;
                if (!string.IsNullOrEmpty(companyIdClaim))
                {
                    context.HttpContext.Items["UserCompanyId"] = int.Parse(companyIdClaim);
                    return;
                }
            }

            // User rolü sadece kendi şubesindeki depolara erişebilir
            if (roles.Contains("User"))
            {
                var companyIdClaim = user.FindFirst("companyId")?.Value;
                var branchIdClaim = user.FindFirst("branchId")?.Value;
                
                if (!string.IsNullOrEmpty(companyIdClaim) && !string.IsNullOrEmpty(branchIdClaim))
                {
                    context.HttpContext.Items["UserCompanyId"] = int.Parse(companyIdClaim);
                    context.HttpContext.Items["UserBranchId"] = int.Parse(branchIdClaim);
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}