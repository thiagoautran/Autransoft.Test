using Autransoft.SendAsync.Mock.Lib.Servers;
using Autransoft.Test.Lib.Data;
using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Autransoft.Test.Lib.Program
{
    public class BaseClassTest<ITestClass, Entity, IEntityTypeConfiguration> : IDisposable
        where ITestClass : class
        where Entity : class
        where IEntityTypeConfiguration : IEntityTypeConfiguration<Entity>
    {
        public SendAsyncMethodMock SendAsyncMethodMock { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; set; }

        public IRepository Repository { get; set; }

        private string _environment;

        public ITestClass TestClass 
        { 
            get
            {
                SendAsyncMethodMock.AddToDependencyInjection(ServiceCollection);

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
            _environment = "IntegrationTest";

            SqlLiteContext.Assembly = typeof(IEntityTypeConfiguration).Assembly;

            ServiceCollection = new ServiceCollection();
            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public BaseClassTest(string environment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
            _environment = environment;

            SqlLiteContext.Assembly = typeof(IEntityTypeConfiguration).Assembly;

            ServiceCollection = new ServiceCollection();
            Configuration = (new ConfigurationBuilder().AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: false)).Build();

            SendAsyncMethodMock = new SendAsyncMethodMock();
        }

        public void Initialize()
        {
            ServiceCollection.AddDbContext<SqlLiteContext>(options => options.UseSqlite($"Data Source={SqlLiteContext.SQL_LITE_DB_NAME}.db"));

            ServiceCollection.AddScoped(typeof(IRepository), typeof(RepositoryBefore));

            AddToDependencyInjection(ServiceCollection, Configuration);

            ServiceProvider = ServiceCollection.BuildServiceProvider();

            Repository = ServiceProvider.GetService<IRepository>();
        }

        public virtual void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) { }
        
        public void Dispose()
        {
            SendAsyncMethodMock.Dispose();

            SqlLiteDispose();
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
    }
}