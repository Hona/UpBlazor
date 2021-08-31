using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace UpBlazor.Web.Controllers
{
    [ApiController]
    [Route("/signin")]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Signin([FromQuery] string @return = "/")
        {
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                return LocalRedirect(@return);
            }

            return Challenge();
        }

        [HttpGet("/signout")]
        public async Task<IActionResult> SignoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return LocalRedirect("/signout/success");
        }
    }
}