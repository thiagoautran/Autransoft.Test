using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class Repository<EntityFrameworkDbContext>
        where EntityFrameworkDbContext : DbContext
    {
        public SqlLiteContext<EntityFrameworkDbContext> DbContext { get; set; }

        public Repository(SqlLiteContext<EntityFrameworkDbContext> dbContext) => DbContext = dbContext;
    }
}