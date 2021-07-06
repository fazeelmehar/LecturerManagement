using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using UniversityEnrollmentManager.Data.Context;
using UniversityEnrollmentManager.Data.Settings;

namespace UniversityEnrollmentManager.Data.Providers
{
    public interface IDataProvider
    {
        DataProvider Provider { get; }
        IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString);
    }

    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;
        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            if (Provider == DataProvider.MSSQL)
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(connectionString, x =>
                    {
                        x.MigrationsAssembly("UniversityEnrollmentManager.Data");
                        x.CommandTimeout(1200);
                    }));
            }
            return services;
        }
    }
}
