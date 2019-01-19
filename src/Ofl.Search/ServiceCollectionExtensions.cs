using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ofl.Search
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearchIndexManager(this IServiceCollection serviceCollection)
        {
            // For ease-of-use.
            var sc = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            // Add the index.
            sc = sc.AddTransient<IIndexManager, IndexManager>();

            // Return the service collection.
            return sc;
        }
    }
}
