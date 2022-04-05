using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Template.EntityFramework.Lib.Data;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.Infrastructure.Data
{
    public class ActionRepository : AutranSoftEfRepository<ActionEntity, ActionRepository>, IActionRepository
    {
        public ActionRepository(IAutranSoftEfLogger<ActionRepository> logger, IAutranSoftEfContext dbContext) : base(logger, dbContext) { }

        public async Task<IEnumerable<ActionEntity>> ListAsync() =>
            await _dbContext.Set<ActionEntity>()
                .ToListAsync();
    }
}