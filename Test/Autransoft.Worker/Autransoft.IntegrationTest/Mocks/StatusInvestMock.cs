using Autransoft.ApplicationCore.DTOs;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Infrastructure.Integrations.Skokka;
using Autransoft.SendAsync.Mock.Lib.Entities;
using Autransoft.SendAsync.Mock.Lib.Enums;
using Autransoft.SendAsync.Mock.Lib.Servers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Autransoft.IntegrationTest.Mocks
{
    public class StatusInvestMock : SendAsyncServerMock<IStatusInvestIntegration, StatusInvestIntegration>
    {
        private IEnumerable<AdvancedSearchResultDto> _actions;
        private IEnumerable<AdvancedSearchResultDto> _fiis;

        public override void MockInitialize() 
        {
            var actionsBytes = JsonResource.Actions;
            var actionsStr = Encoding.ASCII.GetString(actionsBytes);
            _actions = JsonSerializer.Deserialize<IEnumerable<AdvancedSearchResultDto>>(actionsStr);

            var fiisBytes = JsonResource.FIIs;
            var fiisStr = Encoding.ASCII.GetString(fiisBytes);
            _fiis = JsonSerializer.Deserialize<IEnumerable<AdvancedSearchResultDto>>(fiisStr);
        }

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
            var index = query.IndexOf("CategoryType=");
            var categoryType = query.Substring(index + "CategoryType=".Length, 1);

            if (!int.TryParse(categoryType, out int category))
                return null;

            if (category == 1)
            {
                return new ResponseMockEntity
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Obj = _actions,
                    SerializationType = SerializationType.Microsoft
                };
            }
            else
            {
                return new ResponseMockEntity
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Obj = _fiis,
                    SerializationType = SerializationType.Microsoft
                };
            }
        }
    }
}