using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Autransoft.SendAsync.Mock.Lib.Servers;
using Autransoft.Test.Lib.Data;
using Autransoft.Test.Lib.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup, Entity, IEntityTypeConfiguration, IRedisDatabase> : IDisposable
        where Startup : class
        where Entity : class
        where IEntityTypeConfiguration : IEntityTypeConfiguration<Entity>
        where IRedisDatabase : StackExchange.Redis.Extensions.Core.Abstractions.IRedisDatabase
    {
        public StackExchange.Redis.Extensions.Core.Abstractions.IRedisDatabase RedisDatabase { get; set; }

        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; set; }

        public IRepository Repository { get; set; }

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

            SqlLiteContext.Assembly = typeof(IEntityTypeConfiguration).Assembly;
            
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

            SqlLiteContext.Assembly = typeof(IEntityTypeConfiguration).Assembly;

            ServiceCollection = new ServiceCollection();
            var configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public void Initialize()
        {
            ServiceCollection.AddDbContext<SqlLiteContext>(options => options.UseSqlite($"Data Source={SqlLiteContext.SQL_LITE_DB_NAME}.db"));

            ServiceCollection.AddScoped(typeof(IRepository), typeof(RepositoryBefore));

            ServiceProvider = ServiceCollection.BuildServiceProvider();

            Repository = ServiceProvider.GetService<IRepository>();
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

                    serviceCollection.AddDbContext<SqlLiteContext>(options => options.UseSqlite($"Data Source={SqlLiteContext.SQL_LITE_DB_NAME}.db"));

                    serviceCollection.AddScoped(typeof(IRepository), typeof(RepositoryAfter));

                    AddToDependencyInjection(serviceCollection, hostBuilderContext.Configuration);

                    ServiceCollection = serviceCollection;
                    Configuration = hostBuilderContext.Configuration;
                });

            var task = hostBuilder.StartAsync();
            task.Wait();

            ServiceProvider = task.Result.Services;

            Repository = ServiceProvider.GetService<IRepository>();
            
            return task.Result;
        }

        public virtual void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) { }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();

            RedisInMemory.Clean();

            HttpClientDispose();

            SqlLiteDispose();

            HostDispose();
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

        private void SqlLiteDispose()
        {
            try
            {
                if (Repository != null && Repository.DbContext != null && Repository.DbContext.Database != null)
                {
                    var task = Repository.DbContext.Database.EnsureDeletedAsync();
                    task.Wait();
                }
            }
            catch
            {
                Console.WriteLine("Ocorreu um erro ao tentar deletar o banco de dados do SqlLite.");
            }
        }

        private void HttpClientDispose()
        {
            if(_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }
}