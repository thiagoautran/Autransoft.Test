using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class SqlLiteContext : DbContext
    {
        internal static Assembly Assembly { get; set; }

        public SqlLiteContext(DbContextOptions<SqlLiteContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly);
        }
    }
}