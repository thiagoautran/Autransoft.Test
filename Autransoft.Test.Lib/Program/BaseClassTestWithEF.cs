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
        public IRepository<EntityFrameworkDbContext> Repository { get; set; }

        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

        public IConfiguration Configuration { get; set; }

        public ITestClass TestClass 
        { 
            get
            {
                RedisInMemory.AddToDependencyInjection(ServiceCollection);

                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

                ServiceCollection.Remove<DbContext>();
                ServiceCollection.AddDbContext<DbContext>(options => options.UseSqlite("Data Source=Test.db"));

                ServiceProvider = ServiceCollection.BuildServiceProvider();

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

            ServiceCollection = new ServiceCollection();

            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.IntegrationTest.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public BaseClassTest(string environment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);

            ServiceCollection = new ServiceCollection();

            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();

            RedisDatabase = new RedisDatabaseRepository();
        }

        public void Initialize(string environment)
        {
            ServiceCollection.AddDbContext<DbContext>(options => options.UseSqlite("Data Source=Test.db"));

            ServiceCollection.AddScoped(typeof(IRepository<EntityFrameworkDbContext>), typeof(Repository<EntityFrameworkDbContext>));

            ServiceProvider = ServiceCollection.BuildServiceProvider();

            Repository = ServiceProvider.GetService<IRepository<EntityFrameworkDbContext>>();
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();
            RedisInMemory.Clean();
        }
    }
}