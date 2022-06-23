using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UpBlazor.WebApi.Controllers
{
    [ApiController]
    [Route("/signin")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private async Task InternalSignoutAsync() => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signin([FromQuery] string @return = "/home")
        {
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                return LocalRedirect(@return ?? "/home");
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