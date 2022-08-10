using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UpBlazor.WebApi.Controllers
{
    [ApiController]
    [Route("api/signin")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task InternalSignoutAsync() => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signin([FromQuery] string? @return = "/home")
        {
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                var expirationClaim = HttpContext.User.FindFirst(ClaimTypes.Expiration);

                DateTime authExpires;

                if (!string.IsNullOrWhiteSpace(expirationClaim?.Value))
                {
                    authExpires = DateTime.Parse(expirationClaim.Value);
                }
                else
                {
                    authExpires = DateTime.Now.AddDays(1);
                }
                
                var baseUri = new Uri(_configuration["UiUri"], UriKind.Absolute);
                var redirect = "/authentication/logged-in" +
                               $"/{Uri.EscapeDataString(HttpContext.User.Identity.Name ?? "Anonymous")}" +
                               $"/{Uri.EscapeDataString(authExpires.ToString("o"))}" +
                               $"/{Uri.EscapeDataString(@return ?? "/home")}";

                return Redirect(new Uri(baseUri, redirect).ToString());
            }

            return Challenge();
        }

        [HttpGet("/signout")]
        public async Task<IActionResult> SignoutAsync()
        {
            await InternalSignoutAsync();
            
            return LocalRedirect("/signout/success");
        }

        [HttpGet("/switch-user")]
        public async Task<IActionResult> SwitchUserAsync()
        {
            await InternalSignoutAsync();

            return Signin();
        }
    }
}