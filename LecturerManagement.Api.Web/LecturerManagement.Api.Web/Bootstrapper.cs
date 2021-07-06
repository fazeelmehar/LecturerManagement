using LecturerManagement.Data.Context;
using LecturerManagement.Data.Extensions;
using LecturerManagement.Data.Settings;
using LecturerManagement.Infrastructure.Bootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LecturerManagement.Api.Web
{
    public static class Bootstrapper
    {
        public static void RegisterEF(IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<Data.Settings.Data>(c =>
            {
                c.Provider = (DataProvider)Enum.Parse(
                    typeof(DataProvider),
                    configuration.GetSection("Data")["Provider"]);
            });

            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            services.AddEntityFramework(configuration);
        }
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                             new Info
                             {
                                 Title = "Lecturer Management - V1",
                                 Version = "v1",
                                 Description = "Lecturer Management Documentation"
                             });
                c.EnableAnnotations();
            });
        }
        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {

            var options = builder.ApplicationServices.GetService<IConfiguration>()
                .GetSection("swagger").Get<Infrastructure.Swagger.SwaggerOptions>();
            if (!options.Enabled)
            {
                return builder;
            }

            var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;

            builder.UseStaticFiles()
                .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

            return options.ReDocEnabled
                 ? builder.UseReDoc(c =>
                 {
                     c.RoutePrefix = routePrefix;
                     c.SpecUrl = $"{options.Name}/swagger.json";
                 })
                 : builder.UseSwaggerUI(c =>
                 {
                     c.DocumentTitle = options.Title;
                     c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                     c.RoutePrefix = routePrefix;
                     c.DocExpansion(DocExpansion.None);
                     c.DefaultModelExpandDepth(2);
                     c.DefaultModelRendering(ModelRendering.Model);
                     c.DefaultModelsExpandDepth(-1);
                     c.DisplayOperationId();
                     c.DisplayRequestDuration();
                     c.EnableDeepLinking();
                     c.EnableFilter();
                     c.ShowExtensions();
                     c.EnableValidator();
                 });
        }

        public static void RegisterApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddUnitOfWork<DataContext>();
            //services.AddTransient(typeof(IPasswordHasher), typeof(PasswordHasher));
            //services.TryAddTransient<IMigration, DatabaseMigrations>();

        }
        public static void CorsConfiguration(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
                   .AllowCredentials()
                   .SetPreflightMaxAge(TimeSpan.FromMinutes(5))
                   //.WithExposedHeaders("ETag")
               );
            });
        public static void RegisterApplications(this IApplicationBuilder builder) => builder.UseSwaggerDocs();

    }
}
