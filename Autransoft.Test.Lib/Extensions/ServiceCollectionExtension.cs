using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Test.Lib.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void Remove<CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(CLASS));

            if(descriptor != null)
                serviceCollection.Remove(descriptor);
        }

        public static void ReplaceSingleton<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddSingleton<INTERFACE, CLASS>();
            }
        }

        public static void ReplaceSingleton<INTERFACE, CLASS>(this IServiceCollection serviceCollection, CLASS clas)
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddSingleton<INTERFACE>(serviceCollection => clas);
            }
        }

        public static void ReplaceTransient<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddTransient<INTERFACE, CLASS>();
            }
        }

        public static void ReplaceTransient<INTERFACE, CLASS>(this IServiceCollection serviceCollection, CLASS clas)
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
                serviceCollection.Remove(descriptor);

            serviceCollection.AddTransient<INTERFACE>(serviceCollection => clas);
        }

        public static void ReplaceScoped<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddScoped<INTERFACE, CLASS>();
            }
        }

        public static void ReplaceScoped<INTERFACE, CLASS>(this IServiceCollection serviceCollection, CLASS clas)
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddScoped<INTERFACE>(serviceCollection => clas);
            }
        }
    }
}