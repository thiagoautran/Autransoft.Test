using Autransoft.ApplicationCore.AppSettings;
using Autransoft.ApplicationCore.Facades;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.ApplicationCore.Services;
using Autransoft.Fluent.HttpClient.Lib.Interfaces;
using Autransoft.Infrastructure.Data;
using Autransoft.Infrastructure.Integrations.Skokka;
using Autransoft.Infrastructure.Logging;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Data;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Helpers;
using Autransoft.Worker.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Autransoft.Worker.Configurations
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public virtual AppSettings AddAppSettings(IServiceCollection services, IConfiguration configuration)
        {
            AutranSoft.LoadDatabase(services, configuration);

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            var appSettings = new AppSettings();
            new ConfigureFromConfigurationOptions<AppSettings>(configuration.GetSection("AppSettings")).Configure(appSettings);

            return appSettings;
        }

        public virtual void AddDatabase(IServiceCollection services) => 
            services.AddScoped<IAutranSoftEfContext, AutranSoftEfContext>();

        public virtual void AddApplicationCore(IServiceCollection services)
        {
            services.AddScoped<IStatusInvestController, StatusInvestController>();

            services.AddScoped<IStatusInvestFacade, StatusInvestFacade>();

            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IFIIService, FIIService>();
        }

        public virtual void AddInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(IAutranSoftFluentLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IAutranSoftEfLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            
            services.AddScoped<IStatusInvestIntegration, StatusInvestIntegration>();

            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IFIIRepository, FIIRepository>();
        }

        public virtual void AddHttpClient(IServiceCollection services, AppSettings appSettings)
        {
            services.AddHttpClient(appSettings.Integrations.StatusInvest.Id, c => 
            {
                c.BaseAddress = new Uri(appSettings.Integrations.StatusInvest.URL);
            });        
        }
    }
}