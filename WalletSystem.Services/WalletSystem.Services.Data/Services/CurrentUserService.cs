using Microsoft.AspNetCore.Http;
using System.Linq;
using WalletSystem.Services.Data.Interfaces;

namespace WalletSystem.Services.Data.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId => _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
    }
}
