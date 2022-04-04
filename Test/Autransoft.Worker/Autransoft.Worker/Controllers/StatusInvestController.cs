using Autransoft.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace Autransoft.Worker.Controllers
{
    public class StatusInvestController : IStatusInvestController
    {
        private readonly IAppLogger<StatusInvestController> _logger;
        private readonly IStatusInvestFacade _statusInvestFacade;

        public StatusInvestController(IAppLogger<StatusInvestController> logger, IStatusInvestFacade statusInvestFacade) =>
            (_logger, _statusInvestFacade) = (logger, statusInvestFacade);

        public async Task SyncActionAndFIIAsync()
        {
            try
            {
                await _statusInvestFacade.SyncActionAndFIIAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}