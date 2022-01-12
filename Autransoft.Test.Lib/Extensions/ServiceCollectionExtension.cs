using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Test.Lib.Extensions
{
    internal static class ServiceCollectionExtension
    {
        internal static void Remove<CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(CLASS));

            if(descriptor != null)
                serviceCollection.Remove(descriptor);
        }
    }
}