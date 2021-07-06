using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UniversityEnrollmentManager.Data.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace UniversityEnrollmentManager.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {

            var dataProviderConfig = configuration.GetSection("Data")["Provider"];
            var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            var dataProviders = currentAssembly.GetImplementationsOf<IDataProvider>();
            var dataProvider = dataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProviderConfig);
            var connectionStringConfig = configuration.GetConnectionString("MSSQLDbs");
            dataProvider?.RegisterDbContext(services, connectionStringConfig);
            return services;
        }

        private static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            return types.Select(type => (T)Activator.CreateInstance(type)).ToList();
        }

    }
}
