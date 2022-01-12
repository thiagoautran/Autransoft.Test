using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class SqlLiteContext<EntityFrameworkDbContext> : DbContext
        where EntityFrameworkDbContext : DbContext
    {
        public SqlLiteContext(DbContextOptions<SqlLiteContext<EntityFrameworkDbContext>> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(EntityFrameworkDbContext).Assembly);
        }
    }
}