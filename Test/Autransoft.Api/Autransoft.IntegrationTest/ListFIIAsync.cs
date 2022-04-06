using Autransoft.Api;
using Autransoft.ApplicationCore.Entities;
using Autransoft.Fluent.HttpClient.Lib.Extensions;
using Autransoft.Infrastructure.Data.Config;
using Autransoft.IntegrationTest.Configurations;
using Autransoft.IntegrationTest.Mocks;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Test.Lib.Extensions;
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
    public class ListFIIAsync : BaseApiTest<Startup<DependencyInjectionConfigTest>, ActionEntity, ActionConfiguration>
    {
        [TestInitialize]
        public void TestInitialize() => base.Initialize();

        [TestCleanup]
        public void TestCleanup() => base.Dispose();

        public override void AddToDependencyInjection(IServiceCollection serviceCollection, IConfiguration configuration) =>
            serviceCollection.ReplaceTransient<IAutranSoftEfContext, SqlLiteContextTest>();

        [TestMethod]
        public async Task HappyDay()
        {
            var fiisBytes = JsonResource.FIIs;
            var fiisStr = Encoding.ASCII.GetString(fiisBytes);
            var advancedSearchResults = JsonSerializer.Deserialize<IEnumerable<AdvancedSearchResultDto>>(fiisStr);

            var fiiRepository = Repository.DbContext.Set<FIIEntity>();

            foreach (var advancedSearchResult in advancedSearchResults)
            {
                var fii = await fiiRepository
                    .Where(action => action.CompanyId == advancedSearchResult.CompanyId)
                    .FirstOrDefaultAsync();

                if (fii == null || fii.CompanyId <= 0)
                    continue;

                await fiiRepository.AddAsync(new FIIEntity
                {
                    Id = Guid.NewGuid(),
                    CompanyId = advancedSearchResult.CompanyId.Value,
                    CompanyName = advancedSearchResult.CompanyName,
                    Ticker = advancedSearchResult.Ticker,
                    LastUpdate = DateTime.UtcNow
                });
            }

            await Repository.DbContext.SaveChangesAsync();

            var fiis = await fiiRepository.ToListAsync();

            var response =  await HttpClient.Fluent().GetAsync("api/v1/statusinvest/fii");
            var data = await response.DeserializeAsync<IEnumerable<FIIEntity>>();

            Assert.IsNotNull(data.HttpStatusCode);
            Assert.AreEqual(HttpStatusCode.OK, data.HttpStatusCode.Value);
            Assert.AreEqual(398, data.Data.Count());
        }
    }
}