using LecturerManagement.Data.Context;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace LecturerManagement.Infrastructure.Bootstrapper
{
    public static class BootstrapperExtensions
    {
   
        public static void AddUnitOfWork<T>(this IServiceCollection services) where T : DataContext
        {
            services.AddTransient<IUnitOfWork<T>, UnitOfWork<T>>();           
            services.AddTransient<IUnitOfWork>(provider => provider.GetService<IUnitOfWork<T>>());
        } 
    }
}
