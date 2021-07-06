using LecturerManagement.Data.Context;
using LecturerManagement.Data.Interface;
using LecturerManagement.Data.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace LecturerManagement.Data.Providers
{
    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;
        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            if (Provider == DataProvider.MSSQL)
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(connectionString, x=>
                    {
                        //x.MigrationsAssembly("LecturerManagement.Data");
                        x.CommandTimeout(1200);
                    }));
            }
            return services;
        }
    }
}
