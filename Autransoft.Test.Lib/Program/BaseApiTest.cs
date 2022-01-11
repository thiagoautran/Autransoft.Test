using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.SendAsync.Mock.Lib.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup> : IDisposable
        where Startup : class
    {
        public WebApplicationFactory<Startup> WebApplicationFactory { get; set; }

        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IRedisDatabase RedisDatabase { get; set; }

        private HttpClient _httpClient;

        public HttpClient HttpClient 
        { 
            get
            {
                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

                if(_httpClient == null)
                    _httpClient = WebApplicationFactory.CreateClient();

                return _httpClient;
            }
        }

        public BaseApiTest(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            WebApplicationFactory = webApplicationFactory;
        }

        public void Initialize()
        {
            RedisInMemory.AddToDependencyInjection(ServiceCollection);
            RedisDatabase = RedisInMemory.Get(ServiceProvider);
            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();
            RedisInMemory.Clean();
            _httpClient = null;
        }
    }
}