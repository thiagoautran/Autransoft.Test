using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Services
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepository;

        public ActionService(IActionRepository actionRepository) =>
            _actionRepository = actionRepository;

        public async Task<IEnumerable<ActionEntity>> ListAsync() =>
            await _actionRepository.ListAsync();
    }
}