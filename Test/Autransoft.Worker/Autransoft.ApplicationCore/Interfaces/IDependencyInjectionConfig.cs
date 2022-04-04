using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IDependencyInjectionConfig
    {
        AppSettings.AppSettings AddAppSettings(IServiceCollection services, IConfiguration configuration);
        void AddDatabase(IServiceCollection services);
        void AddApplicationCore(IServiceCollection services);
        void AddInfrastructure(IServiceCollection services);
        void AddHttpClient(IServiceCollection services, AppSettings.AppSettings appSettings);
    }
}