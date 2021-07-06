using Microsoft.Extensions.DependencyInjection;

namespace UniversityEnrollmentManager.Mapping.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainAutoMapper(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddAutoMapper(typeof(Index));
        }
    }

    public class Index
    {
        public Index() { }
    }
}
