using System;

namespace Autransoft.ApplicationCore.Interfaces
{
    public interface IAppLogger<L>
    {
        void Error(Exception ex, string message = null);
        void Information(string message);
    }
}