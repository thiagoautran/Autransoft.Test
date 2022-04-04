using Autransoft.ApplicationCore.DTOs;
using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Services
{
    public class FIIService : IFIIService
    {
        private readonly IStatusInvestIntegration _statusInvestIntegration;
        private readonly IFIIRepository _fiiRepository;

        public FIIService(IStatusInvestIntegration statusInvestIntegration, IFIIRepository fiiRepository) =>
            (_statusInvestIntegration, _fiiRepository) = (statusInvestIntegration, fiiRepository);

        public async Task<IEnumerable<AdvancedSearchResultDto>> List() => await _statusInvestIntegration.ListAsync(2);

        public async Task Save(IEnumerable<AdvancedSearchResultDto> advancedSearchResults)
        {
            if (advancedSearchResults == null)
                return;

            foreach (var advancedSearchResult in advancedSearchResults)
            {
                if (advancedSearchResult?.CompanyId == null || advancedSearchResult.CompanyId <= 0)
                    continue;

                var fii = await _fiiRepository.GetAsync(advancedSearchResult.CompanyId.Value);
                if (fii == null)
                {
                    await _fiiRepository.AddAsync(new FIIEntity
                    {
                        CompanyId = advancedSearchResult.CompanyId.Value,
                        CompanyName = advancedSearchResult.CompanyName,
                        Ticker = advancedSearchResult.Ticker,
                        LastUpdate = DateTime.UtcNow
                    });
                }
                else
                {
                    fii.LastUpdate = DateTime.UtcNow;
                    await _fiiRepository.UpdateAsync(fii);
                }
            }
        }
    }
}