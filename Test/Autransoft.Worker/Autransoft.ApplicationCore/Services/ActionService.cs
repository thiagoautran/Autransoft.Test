using Autransoft.ApplicationCore.DTOs;
using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Services
{
    public class ActionService : IActionService
    {
        private readonly IStatusInvestIntegration _statusInvestIntegration;
        private readonly IActionRepository _actionRepository;

        public ActionService(IStatusInvestIntegration statusInvestIntegration, IActionRepository actionRepository) =>
            (_statusInvestIntegration, _actionRepository) = (statusInvestIntegration, actionRepository);

        public async Task<IEnumerable<AdvancedSearchResultDto>> List() => await _statusInvestIntegration.ListAsync(1);

        public async Task Save(IEnumerable<AdvancedSearchResultDto> advancedSearchResults)
        {
            if (advancedSearchResults == null)
                return;

            foreach (var advancedSearchResult in advancedSearchResults)
            {
                if (advancedSearchResult?.CompanyId == null || advancedSearchResult.CompanyId <= 0)
                    continue;

                var action = await _actionRepository.GetAsync(advancedSearchResult.CompanyId.Value);
                if (action == null)
                {
                    await _actionRepository.AddAsync(new ActionEntity
                    {
                        CompanyId = advancedSearchResult.CompanyId.Value,
                        CompanyName = advancedSearchResult.CompanyName,
                        Ticker = advancedSearchResult.Ticker,
                        LastUpdate = DateTime.UtcNow
                    });
                }
                else
                {
                    action.LastUpdate = DateTime.UtcNow;
                    await _actionRepository.UpdateAsync(action);
                }
            }
        }
    }
}