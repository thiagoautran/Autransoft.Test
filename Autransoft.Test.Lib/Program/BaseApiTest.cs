using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.SendAsync.Mock.Lib.Base;
using Autransoft.Test.Lib.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Xunit;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup> : IDisposable
        where Startup : class
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

        public IHost Host { get; private set; }

        private HttpClient _httpClient;

        public HttpClient HttpClient 
        { 
            get
            {
                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

                return _httpClient;
            }
        }

        public void Initialize()
        {
            var hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.UseStartup<Startup>();
                webHost.UseEnvironment("IntegrationTest");

                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "IntegrationTest");
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
            });

            Host = hostBuilder.Start();
            
            _httpClient = Host.GetTestClient();

            RedisInMemory.AddToDependencyInjection(ServiceCollection);
            RedisDatabase = RedisInMemory.Get(ServiceProvider);
            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();
            RedisInMemory.Clean();

            Host.WaitForShutdown();
            Host.Dispose();

            if(_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }
}