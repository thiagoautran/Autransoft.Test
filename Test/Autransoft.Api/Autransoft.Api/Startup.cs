using Autransoft.Api.Configurations;
using Autransoft.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Api
{
    public class Startup<I> 
        where I : class, IDependencyInjectionConfig, new()
    {
        public IConfiguration Configuration { get; }
        public I DependencyInjection { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DependencyInjection = new I();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGenSetup();

            DependencyInjection.AddAppSettings(services, Configuration);
            DependencyInjection.AddApplicationCore(services);
            DependencyInjection.AddDatabase(services);
            DependencyInjection.AddInfrastructure(services);

            services.AddControllers();
            services.AddApiVersioning();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerUISetup();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}