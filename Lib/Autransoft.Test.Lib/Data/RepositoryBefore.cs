using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class RepositoryBefore : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public RepositoryBefore(SqlLiteContext dbContext) 
        {
            dbContext.Database.EnsureCreated();

            SqlLiteDispose(dbContext);

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            
            DbContext = dbContext;
        }

        private void SqlLiteDispose(SqlLiteContext dbContext)
        {
            var task = dbContext.Database.EnsureDeletedAsync();
            task.Wait();
        }
    }
}