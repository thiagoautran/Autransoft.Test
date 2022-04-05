using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IDependencyInjectionConfig
    {
        void AddAppSettings(IServiceCollection services, IConfiguration configuration);
        void AddDatabase(IServiceCollection services);
        void AddApplicationCore(IServiceCollection services);
        void AddInfrastructure(IServiceCollection services);
    }
}