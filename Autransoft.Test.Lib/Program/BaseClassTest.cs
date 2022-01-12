using System;
using Autransoft.SendAsync.Mock.Lib.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Test.Lib.Program
{
    public class BaseClassTest<ITestClass> : IDisposable
        where ITestClass : class
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        //public IRedisDatabase RedisDatabase { get; set; }

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

        public void Initialize()
        {
            //RedisInMemory.AddToDependencyInjection(ServiceCollection);
            //RedisDatabase = RedisInMemory.Get(ServiceProvider);
            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();
            //RedisInMemory.Clean();
        }
    }
}