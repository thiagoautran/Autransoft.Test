using Autransoft.ApplicationCore.AppSettings;
using Autransoft.ApplicationCore.DTOs;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Fluent.HttpClient.Lib.Base;
using Autransoft.Fluent.HttpClient.Lib.Helpers;
using Autransoft.Fluent.HttpClient.Lib.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autransoft.Infrastructure.Integrations.Skokka
{
    public class StatusInvestIntegration : HttpClientIntegration<StatusInvestIntegration>, IStatusInvestIntegration
    {
        private readonly string _urlAdvancedSearch;

        public StatusInvestIntegration(IAutranSoftFluentLogger<StatusInvestIntegration> logger, IHttpClientFactory clientFactory, IOptions<AppSettings> appSettings)
            : base(logger, clientFactory, appSettings.Value.Integrations.StatusInvest.Id) =>
            _urlAdvancedSearch = appSettings.Value.Integrations.StatusInvest.Routes.AdvancedSearch;

        public async Task<IEnumerable<AdvancedSearchResultDto>> ListAsync(int categoryType)
        {
            using var client = CreateFluentHttpClient();

            var uri = client.GetUri(_urlAdvancedSearch + "?search={\"Sector\":\"\",\"SubSector\":\"\",\"Segment\":\"\",\"my_range\":\"-20;100\",\"forecast\":{\"upsideDownside\":{\"Item1\":null,\"Item2\":null},\"estimatesNumber\":{\"Item1\":null,\"Item2\":null},\"revisedUp\":true,\"revisedDown\":true,\"consensus\":[]},\"dy\":{\"Item1\":null,\"Item2\":null},\"p_L\":{\"Item1\":null,\"Item2\":null},\"peg_Ratio\":{\"Item1\":null,\"Item2\":null},\"p_VP\":{\"Item1\":null,\"Item2\":null},\"p_Ativo\":{\"Item1\":null,\"Item2\":null},\"margemBruta\":{\"Item1\":null,\"Item2\":null},\"margemEbit\":{\"Item1\":null,\"Item2\":null},\"margemLiquida\":{\"Item1\":null,\"Item2\":null},\"p_Ebit\":{\"Item1\":null,\"Item2\":null},\"eV_Ebit\":{\"Item1\":null,\"Item2\":null},\"dividaLiquidaEbit\":{\"Item1\":null,\"Item2\":null},\"dividaliquidaPatrimonioLiquido\":{\"Item1\":null,\"Item2\":null},\"p_SR\":{\"Item1\":null,\"Item2\":null},\"p_CapitalGiro\":{\"Item1\":null,\"Item2\":null},\"p_AtivoCirculante\":{\"Item1\":null,\"Item2\":null},\"roe\":{\"Item1\":null,\"Item2\":null},\"roic\":{\"Item1\":null,\"Item2\":null},\"roa\":{\"Item1\":null,\"Item2\":null},\"liquidezCorrente\":{\"Item1\":null,\"Item2\":null},\"pl_Ativo\":{\"Item1\":null,\"Item2\":null},\"passivo_Ativo\":{\"Item1\":null,\"Item2\":null},\"giroAtivos\":{\"Item1\":null,\"Item2\":null},\"receitas_Cagr5\":{\"Item1\":null,\"Item2\":null},\"lucros_Cagr5\":{\"Item1\":null,\"Item2\":null},\"liquidezMediaDiaria\":{\"Item1\":null,\"Item2\":null},\"vpa\":{\"Item1\":null,\"Item2\":null},\"lpa\":{\"Item1\":null,\"Item2\":null},\"valorMercado\":{\"Item1\":null,\"Item2\":null}}&CategoryType=" + categoryType);

            var response = await client
                .CleanDefaultRequestHeaders()
                .GetAsync(uri);

            var responseObject = await response
                .DeserializeAsync<IEnumerable<AdvancedSearchResultDto>>();

            return responseObject.Data;
        }
    }
}