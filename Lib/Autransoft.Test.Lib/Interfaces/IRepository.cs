using Autransoft.Test.Lib.Data;

namespace Autransoft.Test.Lib.Interfaces
{
    public interface IRepository
    {
        SqlLiteContext DbContext { get; set; }
    }
}