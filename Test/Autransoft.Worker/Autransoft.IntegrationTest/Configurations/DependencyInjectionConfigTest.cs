using Autransoft.ApplicationCore.AppSettings;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Worker.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Autransoft.IntegrationTest.Configurations
{
    internal class DependencyInjectionConfigTest : DependencyInjectionConfig, IDependencyInjectionConfig
    {
        public override AppSettings AddAppSettings(IServiceCollection services, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override void AddDatabase(IServiceCollection services) { }

        public override void AddHttpClient(IServiceCollection services, AppSettings appSettings)
        {
            throw new NotImplementedException();
        }

        public override void AddInfrastructure(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}