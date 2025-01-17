using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                // Check the cookie to confirm being authenticated.
                config.DefaultAuthenticateScheme = "ClientCookie";
                // After signing in deal out a cookie.
                config.DefaultSignInScheme = "ClientCookie";
                // Check if allowed to anything.
                config.DefaultChallengeScheme = "OurServer";
            }).AddCookie("ClientCookie")
            .AddOAuth("OurServer", config =>
            {
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.CallbackPath = "/oauth/callback";
                config.AuthorizationEndpoint = "https://localhost:44368/oauth/authorize";
                config.TokenEndpoint = "https://localhost:44368/oauth/token";

                config.SaveTokens = true;

                config.Events = new OAuthEvents()
                {
                    OnCreatingTicket = context =>
                    {
                        var accessToken = context.AccessToken;
                        var base64payload = accessToken.Split('.')[1];
                        var bytes = Convert.FromBase64String(base64payload);
                        var jsonPayload = Encoding.UTF8.GetString(bytes);
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                        foreach (var claim in claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddHttpClient();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
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
