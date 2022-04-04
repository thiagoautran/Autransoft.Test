using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Infrastructure.Integrations.Skokka;
using Autransoft.SendAsync.Mock.Lib.Entities;
using Autransoft.SendAsync.Mock.Lib.Servers;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Autransoft.IntegrationTest.Mocks
{
    public class StatusInvestMock : SendAsyncServerMock<IStatusInvestIntegration, StatusInvestIntegration>
    {
        public override void MockInitialize() { }

        public override Expression<Func<StatusInvestIntegration, HttpClient>> HttpClientMethod() => integration => integration.CreateHttpClient();

        public override ResponseMockEntity ConfigureResponseMock(HttpMethod httpMethod, HttpRequestHeaders httpRequestHeaders, string absolutePath, string query, string json)
        {
            if(httpMethod == HttpMethod.Get && absolutePath.ToUpper().Contains("/category/advancedsearchresult".ToUpper()))
            {
                return AdvancedSearch(httpMethod, httpRequestHeaders, absolutePath, query, json);
            }

            return null;
        }

        public ResponseMockEntity AdvancedSearch(HttpMethod httpMethod, HttpRequestHeaders httpRequestHeaders, string absolutePath, string query, string json)
        {
            return new ResponseMockEntity
            {
                HttpStatusCode = HttpStatusCode.OK,
                Obj = null
            };
        }
    }
}