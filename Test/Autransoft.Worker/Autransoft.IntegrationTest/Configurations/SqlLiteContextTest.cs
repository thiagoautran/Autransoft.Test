using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Test.Lib.Data;

namespace Autransoft.IntegrationTest.Configurations
{
    public class SqlLiteContextTest : SqlLiteContext, IAutranSoftEfContext
    {
    }
}