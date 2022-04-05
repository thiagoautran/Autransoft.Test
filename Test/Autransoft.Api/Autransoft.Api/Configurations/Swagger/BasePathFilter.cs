using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.RegularExpressions;

namespace Autransoft.Api.Configurations.Swagger
{
    public class BasePathFilter : IDocumentFilter
    {
        public string BasePath { get; }

        public BasePathFilter(string basePath)
        {
            BasePath = basePath;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var basePath = $"{this.BasePath}/{swaggerDoc.Info.Version}";
            swaggerDoc.Servers.Add(new OpenApiServer() { Url = basePath });
            var pathsToModify = swaggerDoc.Paths.Where(p => p.Key.StartsWith(basePath)).ToList();

            foreach (var path in pathsToModify)
            {
                if (path.Key.StartsWith(this.BasePath))
                {
                    string newKey = Regex.Replace(path.Key, $"^{basePath}", string.Empty);
                    swaggerDoc.Paths.Remove(path.Key);
                    swaggerDoc.Paths.Add(newKey, path.Value);
                }
            }

        }
    }
}