using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Template.EntityFramework.Lib.Data;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Autransoft.Infrastructure.Data
{
    public class FIIRepository : AutranSoftEfRepository<FIIEntity, FIIRepository>, IFIIRepository
    {
        public FIIRepository(IAutranSoftEfLogger<FIIRepository> logger, IAutranSoftEfContext dbContext) : base(logger, dbContext) { }

        public async Task<FIIEntity> GetAsync(int companyId) =>
            await _dbContext.Set<FIIEntity>()
                .Where(action => action.CompanyId == companyId)
                .FirstOrDefaultAsync();
    }
}