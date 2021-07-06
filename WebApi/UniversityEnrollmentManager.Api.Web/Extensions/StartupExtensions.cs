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
using System.Reflection;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Data.Context;
using UniversityEnrollmentManager.Data.Extensions;
using UniversityEnrollmentManager.Data.Providers;
using UniversityEnrollmentManager.Data.Settings;
using UniversityEnrollmentManager.Infrastructure.UnitOfWork;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Api.Web.Extensions
{
    public static class AppStartup
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
    }

    public static class StartupExtensions
    {
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

            return builder.UseSwaggerUI(c =>
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
        }

        public static void AddUnitOfWork<T>(this IServiceCollection services) where T : DataContext
        {
            services.AddTransient<IUnitOfWork<T>, UnitOfWork<T>>();
            services.AddTransient<IUnitOfWork>(provider => provider.GetService<IUnitOfWork<T>>());
        }

        public static IServiceCollection AddEntityServices(this IServiceCollection services)
        {
            return services;
        }

        static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            return types.Select(type => (T)Activator.CreateInstance(type)).ToList();
        }
    }
}
