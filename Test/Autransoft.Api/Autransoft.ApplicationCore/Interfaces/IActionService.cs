using Autransoft.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IActionService
    {
        Task<IEnumerable<ActionEntity>> ListAsync();
    }
}