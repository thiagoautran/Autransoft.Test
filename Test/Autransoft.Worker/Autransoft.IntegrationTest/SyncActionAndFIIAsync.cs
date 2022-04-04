using Autransoft.ApplicationCore.Interfaces;
using Autransoft.IntegrationTest.Mocks;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Data;
using Autransoft.Test.Lib.Program;
using Autransoft.Worker;
using Autransoft.Worker.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Autransoft.IntegrationTest
{
    [TestClass]
    public class SyncActionAndFIIAsync : BaseClassTest<IStatusInvestController, AutranSoftEfContext>
    {
        [TestInitialize]
        public void TestInitialize() => base.Initialize();

        [TestCleanup]
        public void TestCleanup() => base.Dispose();

        public override void AddToDependencyInjection(IServiceCollection serviceCollection)
        {
            new Startup<DependencyInjectionConfig>(serviceCollection, hostContext.Configuration)
                .ConfigureService()
                .CreateServiceProviderBuilder();

            new StatusInvestMock().AddToDependencyInjection(serviceCollection);
        }

        [TestMethod]
        public async Task HappyDay()
        {
            await TestClass.SyncActionAndFIIAsync();
        }
    }
}