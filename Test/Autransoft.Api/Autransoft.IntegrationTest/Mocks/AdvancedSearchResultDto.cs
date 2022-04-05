using System.Text.Json.Serialization;

namespace Autransoft.IntegrationTest.Mocks
{
    public class AdvancedSearchResultDto
    {
        [JsonInclude]
        [JsonPropertyName("companyId")]
        public int? CompanyId { get; set; }

        [JsonInclude]
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonInclude]
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
    }
}