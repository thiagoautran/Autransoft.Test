using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Autransoft.SendAsync.Mock.Lib.Servers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Autransoft.Test.Lib.Program
{
    public class BaseClassTest<ITestClass, IRedisDatabase> : IDisposable
        where ITestClass : class
        where IRedisDatabase : StackExchange.Redis.Extensions.Core.Abstractions.IRedisDatabase
    {
        public StackExchange.Redis.Extensions.Core.Abstractions.IRedisDatabase RedisDatabase { get; set; }

        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; set; }

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

            ServiceCollection = new ServiceCollection();
            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public void Initialize()
        {
            AddToDependencyInjection(ServiceCollection, Configuration);

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public virtual void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) { }
        
        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();

            RedisInMemory.Clean();
        }
    }
}