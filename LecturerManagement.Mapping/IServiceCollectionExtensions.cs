using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace LecturerManagement.Domain.Mapping
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainAutoMapper(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddAutoMapper(typeof(Index));
        }
    }
}
