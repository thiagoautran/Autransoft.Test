using Autransoft.ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IStatusInvestIntegration
    {
        Task<IEnumerable<AdvancedSearchResultDto>> ListAsync(int categoryType);
    }
}