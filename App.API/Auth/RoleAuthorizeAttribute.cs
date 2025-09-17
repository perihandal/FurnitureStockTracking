using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace App.API.Auth
{
    /// <summary>
    /// Rol bazlı yetkilendirme attribute'u
    /// Belirtilen roller için erişim kontrolü yapar
    /// </summary>
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public RoleAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Authentication kontrolü
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = context.HttpContext.User;
            var userRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // Kullanıcının rollerinden herhangi biri izin verilen roller arasında var mı?
            if (!userRoles.Any(role => _allowedRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}