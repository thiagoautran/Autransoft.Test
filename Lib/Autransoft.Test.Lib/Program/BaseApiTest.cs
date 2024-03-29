using Autransoft.SendAsync.Mock.Lib.Servers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup> : IDisposable
        where Startup : class
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; set; }

        public IHost Host { get; private set; }

        private HttpClient _httpClient;

        private string _environment;

        public HttpClient HttpClient 
        { 
            get
            {
                if (Host == null)
                    Host = CreateHost();

                if (_httpClient == null)
                    _httpClient = Host.GetTestClient();

                return _httpClient;
            }
        }

        public BaseApiTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "IntegrationTest");
            _environment = "IntegrationTest";

            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public BaseApiTest(string environment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
            _environment = environment;

            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public void Initialize()
        {
        }

        private IHost CreateHost()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", _environment);
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", _environment);
                    webHostBuilder.UseEnvironment("IntegrationTest");

                    webHostBuilder.UseTestServer();
                    webHostBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    SendAsyncMethodMock.AddToDependencyInjection(serviceCollection);

                    AddToDependencyInjection(serviceCollection, hostBuilderContext.Configuration);

                    ServiceCollection = serviceCollection;
                    Configuration = hostBuilderContext.Configuration;
                });

            var task = hostBuilder.StartAsync();
            task.Wait();

            ServiceProvider = task.Result.Services;

            return task.Result;
        }

        public virtual void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) { }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();

            HttpClientDispose();

            HostDispose();
        }

        private void HttpClientDispose()
        {
            if(_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }

        private void HostDispose()
        {
            if(Host != null)
            {
                var task = Host.StopAsync();
                task.Wait();

                Host.Dispose();
            }
        }
    }
}