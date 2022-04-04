using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class RepositoryAfter : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public RepositoryAfter(SqlLiteContext dbContext) =>
            DbContext = dbContext;
    }
}