using Autransoft.Worker.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TAN.Core._5._0.Skokka.Pulling.Worker.Workers;

namespace Autransoft.Worker
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    new Startup<DependencyInjectionConfig>(services, hostContext.Configuration)
                        .ConfigureService()
                        .CreateServiceProviderBuilder();

                    services.AddHostedService<StatusInvestWorker>();
                });
    }
}