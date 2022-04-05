using Autransoft.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IStatusInvestFacade
    {
        Task<IEnumerable<ActionEntity>> ListActionAsync();
        Task<IEnumerable<FIIEntity>> ListFIIAsync();
    }
}