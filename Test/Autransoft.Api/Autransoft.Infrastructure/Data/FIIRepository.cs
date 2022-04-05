using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Template.EntityFramework.Lib.Data;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.Infrastructure.Data
{
    public class FIIRepository : AutranSoftEfRepository<FIIEntity, FIIRepository>, IFIIRepository
    {
        public FIIRepository(IAutranSoftEfLogger<FIIRepository> logger, IAutranSoftEfContext dbContext) : base(logger, dbContext) { }

        public async Task<IEnumerable<FIIEntity>> ListAsync() =>
            await _dbContext.Set<FIIEntity>()
                .ToListAsync();
    }
}