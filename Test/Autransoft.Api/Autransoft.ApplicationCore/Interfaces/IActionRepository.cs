using Autransoft.ApplicationCore.Entities;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IActionRepository : IAutranSoftEfRepository<ActionEntity>
    {
        Task<ActionEntity> GetAsync(int companyId);
    }
}