using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;
using Xunit;

namespace App.Services.Tests
{
    public class BaseServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly TestBaseService _baseService;

        public BaseServiceTests()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _baseService = new TestBaseService(_mockHttpContextAccessor.Object);
        }

        [Fact]
        public void IsAdmin_WhenUserHasAdminRole_ReturnsTrue()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.IsAdmin();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEditor_WhenUserHasEditorRole_ReturnsTrue()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Editor")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.IsEditor();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsUser_WhenUserHasUserRole_ReturnsTrue()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.IsUser();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetUserCompanyId_WhenCompanyIdClaimExists_ReturnsCompanyId()
        {
            // Arrange
            var expectedCompanyId = 123;
            var claims = new List<Claim>
            {
                new Claim("CompanyId", expectedCompanyId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.GetUserCompanyId();

            // Assert
            Assert.Equal(expectedCompanyId, result);
        }

        [Fact]
        public void GetUserBranchId_WhenBranchIdClaimExists_ReturnsBranchId()
        {
            // Arrange
            var expectedBranchId = 456;
            var claims = new List<Claim>
            {
                new Claim("BranchId", expectedBranchId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.GetUserBranchId();

            // Assert
            Assert.Equal(expectedBranchId, result);
        }

        [Fact]
        public void ValidateEntityAccess_AdminUser_ReturnsSuccess()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.ValidateEntityAccess(123, 456);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ValidateEntityAccess_EditorWithMatchingCompany_ReturnsSuccess()
        {
            // Arrange
            var userCompanyId = 123;
            var userBranchId = 456;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Editor"),
                new Claim("CompanyId", userCompanyId.ToString()),
                new Claim("BranchId", userBranchId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.ValidateEntityAccess(userCompanyId, userBranchId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ValidateEntityAccess_EditorWithDifferentCompany_ReturnsFail()
        {
            // Arrange
            var userCompanyId = 123;
            var userBranchId = 456;
            var entityCompanyId = 999; // Different company
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Editor"),
                new Claim("CompanyId", userCompanyId.ToString()),
                new Claim("BranchId", userBranchId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = _baseService.ValidateEntityAccess(entityCompanyId, userBranchId);

            // Assert
            Assert.False(result.IsSuccess);
        }
    }

    // Test wrapper class to expose protected methods
    public class TestBaseService : BaseService
    {
        public TestBaseService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        // Expose protected methods as public for testing
        public new bool IsAdmin() => base.IsAdmin();
        public new bool IsEditor() => base.IsEditor();
        public new bool IsUser() => base.IsUser();
        public new int GetUserCompanyId() => base.GetUserCompanyId();
        public new int GetUserBranchId() => base.GetUserBranchId();
        public new ServiceResult ValidateEntityAccess(int entityCompanyId, int? entityBranchId) => 
            base.ValidateEntityAccess(entityCompanyId, entityBranchId);
    }
}
