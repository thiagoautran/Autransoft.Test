using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IStatusInvestFacade
    {
        Task SyncActionAndFIIAsync();
    }
}