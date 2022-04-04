using Autransoft.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autransoft.Worker
{
    public class Startup<DependencyInjection>
        where DependencyInjection : class, IDependencyInjectionConfig, new()
    {
        protected IDependencyInjectionConfig _dependencyInjection;

        public IServiceCollection ServiceCollection { get; private set; }
        public ServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public Startup(IServiceCollection services, IConfiguration configuration)
        {
            _dependencyInjection = new DependencyInjection();
            ServiceCollection = services;
            ServiceProvider = null;
            Configuration = configuration;
        }

        public Startup<DependencyInjection> ConfigureService()
        {
            var appSettings = _dependencyInjection.AddAppSettings(ServiceCollection, Configuration);
            _dependencyInjection.AddDatabase(ServiceCollection);
            _dependencyInjection.AddApplicationCore(ServiceCollection);
            _dependencyInjection.AddInfrastructure(ServiceCollection);
            _dependencyInjection.AddHttpClient(ServiceCollection, appSettings);

            ServiceCollection.AddLogging(logging =>
            {
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            return this;
        }

        public Startup<DependencyInjection> CreateServiceProviderBuilder()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();

            return this;
        }
    }
}