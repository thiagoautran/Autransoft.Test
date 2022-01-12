using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.SendAsync.Mock.Lib.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;

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

                ServiceProvider = ServiceCollection.BuildServiceProvider();
                
                return _httpClient;
            }
        }

        public void Initialize()
        {
            Host = CreateHost();

            _httpClient = Host.GetTestClient();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            //RedisInMemory.AddToDependencyInjection(ServiceCollection);

            ServiceProvider = ServiceCollection.BuildServiceProvider();

            //RedisDatabase = RedisInMemory.Get(ServiceProvider);
        }

        private IHost CreateHost()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder.UseTestServer();
                    webHostBuilder.UseStartup<Startup>();
                    webHostBuilder.UseEnvironment("IntegrationTest");
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    ServiceCollection = serviceCollection;
                });

            var task = hostBuilder.StartAsync();
            task.Wait();
            return task.Result;
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