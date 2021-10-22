using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UpBlazor.Application.Services;
using UpBlazor.Infrastructure;
using UpBlazor.Infrastructure.EfCore;

namespace UpBlazor.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();

            services.AddAuthentication(MicrosoftAccountDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/access-denied";
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.AuthorizationEndpoint = MicrosoftAccountDefaults.AuthorizationEndpoint + "?prompt=select_account";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.AllowedEmailAuthorizationPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Email)
                        .RequireClaim(ClaimTypes.GivenName)
                        .RequireAssertion(context =>
                        {
                            var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                            var allowedEmails = Configuration
                                .GetSection(Constants.AllowedEmailAuthorizationPolicy)
                                .GetChildren()
                                .Select(x => x.Value)
                                .ToArray();

                            if (!allowedEmails.Any())
                            {
                                return true;
                            }

                            return allowedEmails.Any(x => x == emailAddress);
                        });
                });
                
                options.AddPolicy(Constants.AdminAuthorizationPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Email)
                        .RequireAssertion(context =>
                        {
                            var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
                                ?.Value;
                            
                            var adminEmails = Configuration
                                .GetSection(Constants.AdminAuthorizationPolicy)
                                .GetChildren()
                                .Select(x => x.Value)
                                .ToArray();

                            if (!adminEmails.Any())
                            {
                                return false;
                            }

                            return adminEmails.Any(x => x == emailAddress);
                        });
                });
            });

            //services.AddMarten(Configuration);
            services.AddEfCore(Configuration, DbType.Sqlite);

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<INormalizerService, NormalizerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // All requests are forced through NGINX, we can clear all network/proxy restrictions
            var forwardedOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwardedOptions.KnownNetworks.Clear();
            forwardedOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedOptions);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}