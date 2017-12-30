using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ofl.Search
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearch(this IServiceCollection serviceCollection)
        {
            // Validate parameters.
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            // For ease-of-use.
            var sc = serviceCollection;

            // Add the index.
            sc = sc.AddTransient<IIndexManager, IndexManager>();

            // Return the service collection.
            return sc;
        }
    }
}
