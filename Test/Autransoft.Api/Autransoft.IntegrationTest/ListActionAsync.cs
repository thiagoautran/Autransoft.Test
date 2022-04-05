using Autransoft.Api;
using Autransoft.ApplicationCore.Entities;
using Autransoft.Fluent.HttpClient.Lib.Extensions;
using Autransoft.Infrastructure.Data.Config;
using Autransoft.IntegrationTest.Configurations;
using Autransoft.IntegrationTest.Mocks;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Test.Lib.Program;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Autransoft.IntegrationTest
{
    [TestClass]
    public class ListActionAsync : BaseApiTest<Startup<DependencyInjectionConfigTest>, ActionEntity, EscortConfiguration>
    {
        [TestInitialize]
        public void TestInitialize() => base.Initialize();

        [TestCleanup]
        public void TestCleanup() => base.Dispose();

        public override void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) =>
            serviceCollection.AddSingleton<IAutranSoftEfContext, SqlLiteContextTest>();

        [TestMethod]
        public async Task HappyDay()
        {
            var actionsBytes = JsonResource.Actions;
            var actionsStr = Encoding.ASCII.GetString(actionsBytes);
            var advancedSearchResults = JsonSerializer.Deserialize<IEnumerable<AdvancedSearchResultDto>>(actionsStr);

            var actionRepository = Repository.DbContext.Set<ActionEntity>();

            foreach (var advancedSearchResult in advancedSearchResults)
            {
                var action = await actionRepository
                    .Where(action => action.CompanyId == advancedSearchResult.CompanyId)
                    .FirstOrDefaultAsync();

                if (action == null || action.CompanyId <= 0)
                    continue;

                await actionRepository.AddAsync(new ActionEntity
                {
                    Id = Guid.NewGuid(),
                    CompanyId = advancedSearchResult.CompanyId.Value,
                    CompanyName = advancedSearchResult.CompanyName,
                    Ticker = advancedSearchResult.Ticker,
                    LastUpdate = DateTime.UtcNow
                });
            }

            await Repository.DbContext.SaveChangesAsync();

            var response = await HttpClient.Fluent().GetAsync("api/v1/statusinvest/action");
            var data = await response.DeserializeAsync<IEnumerable<ActionEntity>>();

            Assert.IsNotNull(data.HttpStatusCode);
            Assert.AreEqual(HttpStatusCode.OK, data.HttpStatusCode.Value);
            Assert.AreEqual(422, data.Data.Count());
        }
    }
}