using System;
using System.Net.Http;
using Autransoft.Redis.InMemory.Lib.InMemory;
using Autransoft.SendAsync.Mock.Lib.Base;
using Autransoft.Test.Lib.Server;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Xunit;

namespace Autransoft.Test.Lib.Program
{
    public class BaseApiTest<Startup> : IClassFixture<Startup>, IDisposable
        where Startup : class
    {
        public AutransoftServer<Startup> AutransoftServer { get; set; }

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
                    _httpClient = AutransoftServer.CreateClient();

                return _httpClient;
            }
        }

        public BaseApiTest(AutransoftServer<Startup> autransoftServer)
        {
            _httpClient = autransoftServer.CreateClient();
            AutransoftServer = autransoftServer;
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