using Autransoft.Test.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class Repository : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public Repository(SqlLiteContext dbContext) 
        {
            dbContext.Database.Migrate();
            DbContext = dbContext;
        } 
    }
}