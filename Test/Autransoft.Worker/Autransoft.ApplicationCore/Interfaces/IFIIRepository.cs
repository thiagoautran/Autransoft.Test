using Autransoft.ApplicationCore.Entities;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IFIIRepository : IAutranSoftEfRepository<FIIEntity>
    {
        Task<FIIEntity> GetAsync(int companyId);
    }
}