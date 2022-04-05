using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Api.Configurations
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public virtual void AddAppSettings(IServiceCollection services, IConfiguration configuration) { }

        public virtual void AddApplicationCore(IServiceCollection services) { }

        public virtual void AddDatabase(IServiceCollection services) { }

        public virtual void AddInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        }
    }
}