using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Up.NET.Api;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext _httpContext;
        private readonly IUpUserTokenRepository _upUserTokenRepository;
        private IEnumerable<Claim> Claims => _httpContext.User.Claims;

        private UpApi _upApi;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUpUserTokenRepository upUserTokenRepository)
        {
            _upUserTokenRepository = upUserTokenRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<UpApi> GetApiAsync(bool forceReload = false)
        {
            if (!forceReload && _upApi != null)
            {
                return _upApi;
            }
            
            var userId = GetUserId();

            var userToken = await _upUserTokenRepository.GetByUserIdAsync(userId);

            _upApi = new UpApi(userToken.AccessToken);
            return _upApi;
        }

        public string GetUserId() => Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("Logged in user must have a ID claim");
    }
}