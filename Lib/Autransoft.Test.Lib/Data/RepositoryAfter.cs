using Autransoft.Test.Lib.Interfaces;

namespace Autransoft.Test.Lib.Data
{
    public class RepositoryAfter : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public RepositoryAfter(SqlLiteContext dbContext) =>
            DbContext = dbContext;
    }
}