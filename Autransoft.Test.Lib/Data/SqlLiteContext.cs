using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class SqlLiteContext : DbContext
    {
        internal static Assembly Assembly { get; set; }

        public SqlLiteContext() { }

        public SqlLiteContext(DbContextOptions<SqlLiteContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=AutransoftSqlLite.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}