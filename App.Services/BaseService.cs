using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Services
{
    public abstract class BaseService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// HttpContext'ten kullanıcının companyId'sini alır
        /// </summary>
        protected int? GetUserCompanyId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items.ContainsKey("UserCompanyId") == true)
            {
                return (int)httpContext.Items["UserCompanyId"];
            }
            return null;
        }

        /// <summary>
        /// HttpContext'ten kullanıcının branchId'sini alır
        /// </summary>
        protected int? GetUserBranchId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items.ContainsKey("UserBranchId") == true)
            {
                return (int)httpContext.Items["UserBranchId"];
            }
            return null;
        }

        /// <summary>
        /// Kullanıcının rollerini alır
        /// </summary>
        protected List<string> GetUserRoles()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            
            if (user?.Identity?.IsAuthenticated == true)
            {
                return user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            }
            
            return new List<string>();
        }

        /// <summary>
        /// Kullanıcının Admin olup olmadığını kontrol eder
        /// </summary>
        protected bool IsAdmin()
        {
            return GetUserRoles().Contains("Admin");
        }

        /// <summary>
        /// Kullanıcının Editor olup olmadığını kontrol eder
        /// </summary>
        protected bool IsEditor()
        {
            return GetUserRoles().Contains("Editor");
        }

        /// <summary>
        /// Kullanıcının User olup olmadığını kontrol eder
        /// </summary>
        protected bool IsUser()
        {
            return GetUserRoles().Contains("User");
        }

        /// <summary>
        /// Entity'nin kullanıcının erişim yetkisi dahilinde olup olmadığını kontrol eder
        /// </summary>
        protected bool CanAccessEntity(int entityCompanyId, int? entityBranchId = null)
        {
            // Admin her şeye erişebilir
            if (IsAdmin())
                return true;

            var userCompanyId = GetUserCompanyId();
            var userBranchId = GetUserBranchId();

            // CompanyId kontrolü
            if (userCompanyId.HasValue && entityCompanyId != userCompanyId.Value)
                return false;

            // User rolü için BranchId kontrolü de yapılmalı
            if (IsUser() && entityBranchId.HasValue && userBranchId.HasValue)
            {
                return entityBranchId.Value == userBranchId.Value;
            }

            return true;
        }

        /// <summary>
        /// Create/Update işlemlerinde companyId ve branchId doğrulaması yapar
        /// </summary>
        protected ServiceResult ValidateEntityAccess(int requestCompanyId, int? requestBranchId = null)
        {
            // Admin her şeyi yapabilir
            if (IsAdmin())
                return ServiceResult.Success();

            var userCompanyId = GetUserCompanyId();
            var userBranchId = GetUserBranchId();

            // CompanyId kontrolü
            if (userCompanyId.HasValue && requestCompanyId != userCompanyId.Value)
            {
                return ServiceResult.Fail("Bu şirkete ait veriler üzerinde işlem yapma yetkiniz bulunmamaktadır.", System.Net.HttpStatusCode.Forbidden);
            }

            // User rolü için BranchId kontrolü
            if (IsUser() && requestBranchId.HasValue && userBranchId.HasValue)
            {
                if (requestBranchId.Value != userBranchId.Value)
                {
                    return ServiceResult.Fail("Bu şubeye ait veriler üzerinde işlem yapma yetkiniz bulunmamaktadır.", System.Net.HttpStatusCode.Forbidden);
                }
            }

            return ServiceResult.Success();
        }
    }
}
