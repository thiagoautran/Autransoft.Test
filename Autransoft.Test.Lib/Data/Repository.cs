using Autransoft.Test.Lib.Interfaces;

namespace Autransoft.Test.Lib.Data
{
    public class Repository : IRepository
    {
        public SqlLiteContext DbContext { get; set; }

        public Repository(SqlLiteContext dbContext) 
        {
            dbContext.Database.EnsureCreated();
            DbContext = dbContext;
        } 
    }
}