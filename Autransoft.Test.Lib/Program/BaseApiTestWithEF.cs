using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Autransoft.SendAsync.Mock.Lib.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup, EntityFrameworkDbContext> : IDisposable
        where Startup : class
        where EntityFrameworkDbContext : DbContext
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

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

                var redisDatabase = RedisInMemory.Get(Host.Services);
                if(redisDatabase != null)
                    ((RedisDatabaseRepository)redisDatabase).SetDatabase(((RedisDatabaseRepository)RedisDatabase).GetDatabase());

                return _httpClient;
            }
        }

        public BaseApiTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "IntegrationTest");
            _environment = "IntegrationTest";
            
            ServiceCollection = new ServiceCollection();
            var configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.IntegrationTest.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public BaseApiTest(string environment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
            _environment = environment;

            ServiceCollection = new ServiceCollection();
            var configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public void Initialize()
        {
            ServiceCollection.AddDbContext<DbContext>(options => options.UseSqlite("Data Source=Test.db"));

            ServiceProvider = ServiceCollection.BuildServiceProvider();
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
                    RedisInMemory.AddToDependencyInjection(serviceCollection);

                    SendAsyncMethodMock.AddToDependencyInjection(serviceCollection);

                    serviceCollection.AddDbContext<DbContext>(options => options.UseSqlite("Data Source=Test.db"));

                    ServiceCollection = serviceCollection;
                });

            var task = hostBuilder.StartAsync();
            task.Wait();

            ServiceProvider = task.Result.Services;

            return task.Result;
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();
            RedisInMemory.Clean();

            var task = Host.StopAsync();
            task.Wait();

            Host.Dispose();

            if(_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }
}