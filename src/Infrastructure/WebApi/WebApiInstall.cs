using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebApi.Auth;
using WebApi.Auth.Config;
using WebApi.Auth.Scopes;
using WebApi.CustomConverters;

namespace WebApi
{
    public static class WebApiInstall
    {

        public static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // Disable validation so we can handle all errors with the exception handler
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsonConverters = options.SerializerSettings.Converters;
                jsonConverters.Add(new TodoListNameConverter());
                jsonConverters.Add(
                    new StringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {Title = "My API", Version = "v1"});
                c.EnableAnnotations();
            });
        }

        public static void ConfigureAuthentication(IServiceCollection services, IAuthConfig authConfig)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authConfig.GetIssuer();
                    options.Audience = authConfig.GetAudience();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
        }

        public static void ConfigureAuthorization(IServiceCollection services, IAuthConfig authConfig)
        {
            services.AddAuthorization(options =>
            {
                foreach (var scope in Scopes.All)
                {
                    options.AddPolicy(scope,
                        policy => policy.Requirements.Add(new HasScopeRequirement(scope,
                            authConfig.GetIssuer())));
                }
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddScoped<IUserManager, UserManager>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features
                    .Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var response = ExceptionHandlerMapper.Map(exception);
                context.Response.StatusCode = response.Status;

                // TODO find a way to reuse the standard config
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                };

                await context.Response.WriteAsJsonAsync(response, jsonSerializerOptions);
            }));


            app.UseHsts();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }
    }
}