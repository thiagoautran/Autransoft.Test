using Autransoft.Test.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace Autransoft.Test.Lib.Interfaces
{
    public interface IRepository<EntityFrameworkDbContext>
        where EntityFrameworkDbContext : DbContext
    {
        SqlLiteContext<EntityFrameworkDbContext> SqlLiteContext { get; set; }
    }
}