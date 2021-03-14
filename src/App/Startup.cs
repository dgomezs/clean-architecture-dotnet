using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Services;
using Application.Services.Errors;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using Domain.Errors;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebApi.CustomConverters;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env) =>
            Configuration = config;

        public ILifetimeScope? AutofacContainer { get; private set; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {Title = "My API", Version = "v1"});
                c.EnableAnnotations();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.Register(_ => Configuration).AsSelf().SingleInstance();
            builder.RegisterModule(new PersistenceModule());
            builder.RegisterModule(new ApplicationServicesModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features
                    .Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                await WriteExceptionToJson(exception, context);
            }));


            app.UseHsts();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }

        private async Task WriteExceptionToJson(Exception exception, HttpContext context)
        {
            if (exception is DomainException appException)
            {
                context.Response.StatusCode = (int) GetStatusCode(appException);
                await context.Response.WriteAsJsonAsync(new
                {
                    errorKey = appException.ErrorKey,
                    message = appException.Message
                });
            }
            else if (exception is DomainValidationException vException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    errorKey = vException.ErrorKey,
                    errors = vException.InternalValidationException.Errors,
                    message = vException.Message
                });
            }
            else
            {
                await context.Response.WriteAsJsonAsync(new
                    {error = exception.Message});
            }
        }

        private HttpStatusCode GetStatusCode(DomainException appException)
        {
            return appException switch
            {
                EntityExistsException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}