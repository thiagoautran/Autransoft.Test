using Autransoft.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Facades
{
    public class StatusInvestFacade : IStatusInvestFacade
    {
        private readonly IActionService _actionsService;
        private readonly IFIIService _fiiService;

        public StatusInvestFacade(IActionService actionsService, IFIIService fiiService) =>
            (_actionsService, _fiiService) = (actionsService, fiiService);

        public async Task SyncActionAndFIIAsync()
        {
            var actions = await _actionsService.List();
            await _actionsService.Save(actions);

            var fiis = await _fiiService.List();
            await _fiiService.Save(fiis);
        }
    }
}