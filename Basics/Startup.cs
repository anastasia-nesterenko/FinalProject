using Basics.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Basics
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(config =>
            {
                //AuthorizationPolicyBuilder defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //config.DefaultPolicy = defaultAuthBuilder.RequireAuthenticatedUser().Build();

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    //policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews(config =>
            {
                // Global authorization filter.
                AuthorizationPolicyBuilder defaultAuthBuilder = new AuthorizationPolicyBuilder();

                config.Filters.Add(new AuthorizeFilter(defaultAuthBuilder.RequireAuthenticatedUser().Build()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Automatic authentication and the handling of remote authentication requests.
            app.UseAuthentication();

            // Check the permissions.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
