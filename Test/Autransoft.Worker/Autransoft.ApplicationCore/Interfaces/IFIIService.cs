using Autransoft.ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IFIIService
    {
        Task<IEnumerable<AdvancedSearchResultDto>> List();
        Task Save(IEnumerable<AdvancedSearchResultDto> advancedSearchResult);
    }
}