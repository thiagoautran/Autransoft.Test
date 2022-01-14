using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class Repository : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public Repository(SqlLiteContext dbContext) 
        {
            dbContext.Database.EnsureCreated();

            SqlLiteDispose();

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            
            DbContext = dbContext;
        }

        private void SqlLiteDispose()
        {
            var task = DbContext.Database.EnsureDeletedAsync();
            task.Wait();
        }
    }
}