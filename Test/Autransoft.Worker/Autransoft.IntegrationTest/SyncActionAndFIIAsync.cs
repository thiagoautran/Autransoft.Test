using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Infrastructure.Data.Config;
using Autransoft.IntegrationTest.Configurations;
using Autransoft.IntegrationTest.Mocks;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Test.Lib.Program;
using Autransoft.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Autransoft.IntegrationTest
{
    [TestClass]
    public class SyncActionAndFIIAsync : BaseClassTest<IStatusInvestController, ActionEntity, EscortConfiguration>
    {
        [TestInitialize]
        public void TestInitialize() => base.Initialize();

        [TestCleanup]
        public void TestCleanup() => base.Dispose();

        public override void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            new Startup<DependencyInjectionConfigTest>(serviceCollection, configuration)
                .ConfigureService();

            new StatusInvestMock().AddToDependencyInjection(serviceCollection);

            serviceCollection.AddSingleton<IAutranSoftEfContext, SqlLiteContextTest>();
        }

        [TestMethod]
        public async Task HappyDay()
        {
            await TestClass.SyncActionAndFIIAsync();

            var actionRepository = Repository.DbContext.Set<ActionEntity>();
            var actions = actionRepository.ToList();

            var fiiRepository = Repository.DbContext.Set<FIIEntity>();
            var fiis = fiiRepository.ToList();

            Assert.AreEqual(422, actions.Count);
            Assert.AreEqual(398, fiis.Count);
        }
    }
}