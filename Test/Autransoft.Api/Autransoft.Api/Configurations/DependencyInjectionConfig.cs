using Autransoft.ApplicationCore.Facades;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.ApplicationCore.Services;
using Autransoft.Infrastructure.Data;
using Autransoft.Infrastructure.Logging;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Data;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Api.Configurations
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public virtual void AddAppSettings(IServiceCollection services, IConfiguration configuration) =>
            AutranSoft.LoadDatabase(services, configuration);

        public virtual void AddApplicationCore(IServiceCollection services) 
        {
            services.AddScoped<IStatusInvestFacade, StatusInvestFacade>();

            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IFIIService, FIIService>();
        }

        public virtual void AddDatabase(IServiceCollection services) =>
            services.AddScoped<IAutranSoftEfContext, AutranSoftEfContext>();

        public virtual void AddInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(IAutranSoftEfLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IFIIRepository, FIIRepository>();
        }
    }
}