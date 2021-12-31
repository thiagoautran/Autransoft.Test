using System.Net.Http;

namespace Autransoft.Test.Lib.Entities
{
    public class DataResponse<Entity>
    {
        public Entity Obj { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}