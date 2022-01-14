using System;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Autransoft.SendAsync.Mock.Lib.Base;
using Autransoft.Test.Lib.Data;
using Autransoft.Test.Lib.Extensions;
using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Test.Lib.Program
{
    public class BaseClassTest<ITestClass, EntityFrameworkDbContext> : IDisposable
        where ITestClass : class
        where EntityFrameworkDbContext : DbContext
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

        public IConfiguration Configuration { get; set; }

        public IRepository Repository { get; set; }

        private string _environment;

        public ITestClass TestClass 
        { 
            get
            {
                RedisInMemory.AddToDependencyInjection(ServiceCollection);

                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

                ServiceProvider = ServiceCollection.BuildServiceProvider();

                var redisDatabase = RedisInMemory.Get(ServiceProvider);
                if(redisDatabase != null)
                    ((RedisDatabaseRepository)redisDatabase).SetDatabase(((RedisDatabaseRepository)RedisDatabase).GetDatabase());

                var testClass = ServiceProvider.GetService(typeof(ITestClass));
                if(testClass != null)
                    return (ITestClass)testClass;

                return null;
            }
        }

        public BaseClassTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "IntegrationTest");
            _environment = "IntegrationTest";

            SqlLiteContext.Assembly = typeof(EntityFrameworkDbContext).Assembly;

            ServiceCollection = new ServiceCollection();
            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public BaseClassTest(string environment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
            _environment = environment;

            SqlLiteContext.Assembly = typeof(EntityFrameworkDbContext).Assembly;

            ServiceCollection = new ServiceCollection();
            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public void Initialize()
        {
            ServiceCollection.AddDbContext<SqlLiteContext>(options => options.UseSqlite($"Data Source={SqlLiteContext.SQL_LITE_DB_NAME}.db"));

            ServiceCollection.AddScoped(typeof(IRepository), typeof(RepositoryBefore));

            AddToDependencyInjection(ServiceCollection);

            ServiceProvider = ServiceCollection.BuildServiceProvider();

            Repository = ServiceProvider.GetService<IRepository>();
        }

        public virtual void AddToDependencyInjection(IServiceCollection serviceCollection) { }
        
        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();

            RedisInMemory.Clean();
        }
    }
}