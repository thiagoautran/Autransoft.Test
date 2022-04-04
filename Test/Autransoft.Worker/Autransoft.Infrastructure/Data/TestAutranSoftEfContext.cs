using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Autransoft.Template.EntityFramework.PostgreSQL.Lib.Data;
using Microsoft.Extensions.Options;

namespace Autransoft.Infrastructure.Data
{
    public class TestAutranSoftEfContext : AutranSoftEfContext, IAutranSoftEfContext
    {
        public TestAutranSoftEfContext(IAutranSoftEfLogger<AutranSoftEfContext> logger, IOptions<Template.EntityFramework.PostgreSQL.Lib.DTOs.Autransoft> autransoft) : base(logger, autransoft) { }
    }
}