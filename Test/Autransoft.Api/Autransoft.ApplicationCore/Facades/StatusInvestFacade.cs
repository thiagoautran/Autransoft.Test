using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Facades
{
    public class StatusInvestFacade : IStatusInvestFacade
    {
        private readonly IActionService _actionsService;
        private readonly IFIIService _fiiService;

        public StatusInvestFacade(IActionService actionsService, IFIIService fiiService) =>
            (_actionsService, _fiiService) = (actionsService, fiiService);

        public async Task<IEnumerable<ActionEntity>> ListActionAsync() =>
            await _actionsService.ListAsync();

        public async Task<IEnumerable<FIIEntity>> ListFIIAsync() =>
            await _fiiService.ListAsync();
    }
}