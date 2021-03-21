using System.Text.Json.Serialization;
using App;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApi.CustomConverters;

namespace WebApi
{
    public static class Install
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddControllers().AddJsonOptions(options =>
            {
                var jsonConverters = options.JsonSerializerOptions.Converters;
                jsonConverters.Add(new TodoListNameConverter());
                jsonConverters.Add(
                    new JsonStringEnumConverter());
            });
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
                await context.Response.WriteAsJsonAsync(response);
            }));


            app.UseHsts();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}