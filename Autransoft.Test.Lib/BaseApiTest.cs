using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.SendAsync.Mock.Lib.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Test.Lib
{
    public class BaseProgramTest<ITestClass, Startup> : IDisposable
        where ITestClass : class
        where Startup : class
    {
        public WebApplicationFactory<Startup> WebApplicationFactory { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

        public HttpClient HttpClient { get; set; }

        public ITestClass TestClass 
        { 
            get
            {
                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

                var testClass = ServiceProvider.GetService(typeof(ITestClass));

                if(testClass != null)
                    return (ITestClass)testClass;

                return null;
            }
        }

        public BaseProgramTest(WebApplicationFactory<Startup> webApplicationFactory)
        {
            HttpClient = webApplicationFactory.CreateClient();
            WebApplicationFactory = webApplicationFactory;
        }

        public void Initialize()
        {
            RedisInMemory.AddToDependencyInjection(ServiceCollection);
            RedisDatabase = RedisInMemory.Get(ServiceProvider);
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Clean();
            RedisInMemory.Clean();
            HttpClient = null;
        }
    }
}