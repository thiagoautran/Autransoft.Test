using Autransoft.Test.Lib.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Autransoft.Test.Lib.Helper
{
    public class HttpClientHelper
    {
        protected HttpClient Client { get; set; }

        public async Task<DataResponse<Entity>> GetAsync<Entity>(string uri)
        {
            var result = new DataResponse<Entity>();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("IntegrationTest", "IntegrationTest");

            result.HttpResponseMessage = await Client.GetAsync(uri);

            if (result.HttpResponseMessage == null)
                return result;

            if (result.HttpResponseMessage.IsSuccessStatusCode)
            {
                var content = await result.HttpResponseMessage.Content.ReadAsStringAsync();
                if(!string.IsNullOrEmpty(content))
                    result.Obj = JsonConvert.DeserializeObject<Entity>(content);
            }
            
            return result;
        }

        public async Task<DataResponse<Entity>> PostAsync<Entity>(string uri, object requestObject)
        {
            var result = new DataResponse<Entity>();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("IntegrationTest", "IntegrationTest");

            result.HttpResponseMessage = await Client.PostAsync(uri, new StringContent
            (
                JsonConvert.SerializeObject(requestObject), 
                Encoding.UTF8, 
                "application/json"
            ));

            if (result.HttpResponseMessage == null)
                return result;

            if (result.HttpResponseMessage.IsSuccessStatusCode)
            {
                var content = await result.HttpResponseMessage.Content.ReadAsStringAsync();
                if(!string.IsNullOrEmpty(content))
                    result.Obj = JsonConvert.DeserializeObject<Entity>(content);
            }
            
            return result;
        }
    }
}