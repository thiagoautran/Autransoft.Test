using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Data
{
    public class SqlLiteContext : DbContext
    {
        internal static Assembly Assembly { get; set; }

        internal static string SQL_LITE_DB_NAME = "AutransoftSqlLite";

        public SqlLiteContext() : base() { }

        public SqlLiteContext(DbContextOptions<SqlLiteContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={SQL_LITE_DB_NAME}.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly);
        }
    }
}