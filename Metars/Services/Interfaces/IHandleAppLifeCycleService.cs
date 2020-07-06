using System;
using System.Threading.Tasks;

namespace Metars.Services.Interfaces
{
    public interface IHandleAppLifeCycleService
    {
        Task OnAppLaunch();
    }
}
