using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Users;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public AuthenticationController(IConfiguration configuration, IMediator mediator, IAuthorizationService authorizationService)
        {
            _configuration = configuration;
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        private async Task InternalSignoutAsync() => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        [HttpGet("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Signin([FromQuery] string? @return = "/home")
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

                // Check API authorization policy manually
                var allowedPolicies = new List<string>();
                
                var adminPolicyResult = await _authorizationService.AuthorizeAsync(HttpContext.User, null, Constants.AdminAuthorizationPolicy);
                if (adminPolicyResult.Succeeded)
                {
                    allowedPolicies.Add("admin");
                }

                var allowedEmailPolicyResult = await _authorizationService.AuthorizeAsync(HttpContext.User, null, Constants.AllowedEmailAuthorizationPolicy);
                if (allowedEmailPolicyResult.Succeeded)
                {
                    allowedPolicies.Add("allowedEmails");
                }
                
                var baseUri = new Uri(_configuration["UiUri"], UriKind.Absolute);
                var redirect = "/authentication/logged-in" +
                               $"/{Uri.EscapeDataString(HttpContext.User.Identity.Name ?? "Anonymous")}" +
                               $"/{Uri.EscapeDataString(authExpires.ToString("o"))}" +
                               $"/{Uri.EscapeDataString(@return ?? "/home")}" +
                               $"/{string.Join(',', allowedPolicies.Select(Uri.EscapeDataString))}";
                
                // Finally register the user
                await _mediator.Send(new RegisterUserCommand(new RegisteredUser
                {
                    Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    GivenName = HttpContext.User.FindFirstValue(ClaimTypes.GivenName),
                    Email = HttpContext.User.FindFirstValue(ClaimTypes.Email)
                }));

                return Redirect(new Uri(baseUri, redirect).ToString());
            }

            return Challenge();
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignoutAsync()
        {
            await InternalSignoutAsync();
            
            var baseUri = new Uri(_configuration["UiUri"], UriKind.Absolute);
            return Redirect( new Uri(baseUri, "/signout/success").ToString());
        }

        [HttpGet("switch-user")]
        public async Task<IActionResult> SwitchUserAsync()
        {
            await InternalSignoutAsync();

            return await Signin();
        }
    }
}