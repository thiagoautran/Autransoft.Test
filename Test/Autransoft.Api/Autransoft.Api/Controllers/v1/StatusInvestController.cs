using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IStatusInvestFacade _statusInvestFacade;

        public StatusInvestController(IAppLogger<StatusInvestController> logger, IStatusInvestFacade statusInvestFacade) =>
            (_logger, _statusInvestFacade) = (logger, statusInvestFacade);

        [HttpGet("action")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActionEntity>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = null)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = null)]
        public async Task<IActionResult> ListActionAsync()
        {
            try
            {
                var result = await _statusInvestFacade.ListActionAsync();

                if (result == null || result.Count() == 0)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("fii")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FIIEntity>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = null)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = null)]
        public async Task<IActionResult> ListFIIAsync()
        {
            try
            {
                var result = await _statusInvestFacade.ListFIIAsync();

                if (result == null || result.Count() == 0)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}