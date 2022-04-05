using Autransoft.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Autransoft.Api.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/statusinvest")]
    public class StatusInvestController : ControllerBase
    {
        private readonly IAppLogger<StatusInvestController> _logger;

        public StatusInvestController(IAppLogger<StatusInvestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<ContentResult> GetEscortsHtmlAsync()
        {
            try
            {
                return new ContentResult 
                {
                    ContentType = "text/html",
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ContentResult 
                {
                    ContentType = "text/html",
                    Content = string.Empty
                };
            }
        }
    }
}